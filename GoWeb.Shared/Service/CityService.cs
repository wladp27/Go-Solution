using GoWeb.Shared.Model;
using GoWeb.Shared.Models;
using GoWeb.Shared.Requests;
using MediatR;
using System.Threading.Tasks;


namespace GoWeb.Shared.Service
{
    public class CityService
    {
        private List<CityDTO> cityList = default!;

        private readonly IMediator mediator;
       
        public CityService(IMediator mediator)
        {
            this.mediator = mediator;
        }
        public async Task<OperationResult<List<CityDTO>>> GetCities()
        {
            if(cityList == null)
            {
               var response = await mediator.Send(new GetCitiesRequest());
               if(response.IsSuccess)
               {
                   cityList = response.Data.Cities ?? new();
                   return OperationResult<List<CityDTO>>.Success(cityList);
               }
                return OperationResult<List<CityDTO>>.Failure(response.ErrorMessage);
            }
            return OperationResult<List<CityDTO>>.Success(cityList);
        }

        public async Task<OperationResult<CityDTO>> GetCityById(int id)
        {
            var response = await GetCities();
            if (response.IsSuccess)
            {
                var city =  response.Data.FirstOrDefault(c=>c.Id==id);
                if(city == null)
                    return OperationResult<CityDTO>.Failure("Выбранного города не существует в базе приложения");
                return OperationResult<CityDTO>.Success(city);
            }
            return OperationResult<CityDTO>.Failure(response.ErrorMessage);
        }
    }
}
