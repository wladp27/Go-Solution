using AutoMapper;
using GoWeb.Interfaces;
using GoWeb.Models;
using GoWeb.Shared.Models;
using GoWeb.Сonstants.Cache;
using GoWebApplication.Db.Models;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;

namespace GoWeb.Service
{
    public class CityService : ICityService
    {
        private readonly ICityRepository cityRepository;
        private readonly ICacheService cache;
        private readonly IMapper mapper;
        private static readonly SemaphoreSlim semForGetAll = new SemaphoreSlim(1, 1);
        private static readonly ConcurrentDictionary<int, SemaphoreSlim> _semaphoresById = new();
        public CityService(ICityRepository cityRepository, ICacheService cache, IMapper mapper)
        {
            this.cityRepository = cityRepository;
            this.cache = cache;
            this.mapper = mapper;
        }

        public async Task<bool> AddAsync(CityDTO cityView)
        {
            cityView.Id = await cityRepository.AddAsync(mapper.Map<City>(cityView));
            if (cityView.Id > 0)
            {
                await cache.SetAsync(new CityCacheKey(cityView.Id).ToString(), cityView);
                return true;
            }
            return false;

        }

        public async Task<bool> DeleteAsync(CityDTO city)
        {

            var successDelete = await cityRepository.DeleteAsync((mapper.Map<City>(city)));
            if (successDelete)
            {
                await cache.RemoveAsync(new CityCacheKey(city.Id).ToString());
                return true;
            }
            return false;
        }

        public async Task<List<CityDTO>?> GetAllAsync()
        {
            var allCityView = new List<CityDTO>();
            var (isSuccess, cachedCities) = await cache.TryGetValueAsync<List<CityDTO>>(CacheConst.allCities);
            if (isSuccess)
            {
                return cachedCities;
            }
            await semForGetAll.WaitAsync();
            try
            {
                (isSuccess, cachedCities) = await cache.TryGetValueAsync<List<CityDTO>>(CacheConst.allCities);
                if (isSuccess)
                {
                    return cachedCities;
                }
                var allCityDB = await cityRepository.GetAllAsync();
                allCityView = mapper.Map<List<CityDTO>>(allCityDB);
                await cache.SetAsync(CacheConst.allCities, allCityView, new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromDays(1)));
            }
            finally
            {
                semForGetAll.Release();
            }
            return allCityView;
        }

        public async Task<CityDTO?> GetByIdAsync(int id)
        {
            var (isSuccess, cityView) = await cache.TryGetValueAsync<CityDTO>(new CityCacheKey(id).ToString());
            if (isSuccess)
            {
                return cityView;
            }
            var semaphore = _semaphoresById.GetOrAdd(id, _ => new SemaphoreSlim(1, 1));
            await semaphore.WaitAsync();
            try
            {
                (isSuccess, cityView) = await cache.TryGetValueAsync<CityDTO>(new CityCacheKey(id).ToString());
                if (isSuccess)
                {
                    return cityView;
                }
                var cityDB = await cityRepository.GetByIdAsync(id);
                cityView = mapper.Map<CityDTO>(cityDB);
                if (cityView != null)
                {
                    await cache.SetAsync(new CityCacheKey(id).ToString(), cityView);
                }
                else
                {
                    await cache.SetAsync(new CityCacheKey(id).ToString(), cityView, new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(1)));
                }

            }
            finally
            {
                semaphore.Release();
            }
            return cityView;
        }

        public async Task<bool> Update(CityDTO city)
        {
            var updateCityDb = mapper.Map<City>(city);
            var successUpdate = await cityRepository.Update(updateCityDb);
            if (successUpdate)
            {
                await cache.SetAsync(new CityCacheKey(city.Id).ToString(), city);
                return true;
            }
            return false;

        }
        public record CityCacheKey(int id)
        {
            public override string ToString() => $"city:summary:{id}";
        }
    }
}
