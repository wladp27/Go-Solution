using Moq;
using Xunit;
using GoWeb.Service;
using Microsoft.Extensions.Caching.Memory;
using GoWebApplication.Db.Models;
using GoWeb.Interfaces;
using MediatR;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using GoWeb.Сonstants.Cache;

namespace StatusEventTest
{
    public class StatusEventServiceTests
    {
        [Fact]

        public async Task GetByIdAsync_WhenCacheMiss_CallsRepositoryAndSetsCache()
        {
            var mockRepo = new Mock<IStatusEvent>();
            var cache = new MemoryCache(new MemoryCacheOptions());

            int testId = 1;
            var expectedStatus = new StatusEvent { Id = testId, TypeStatus = "Canceled" };

            mockRepo.Setup(r => r.GetByIdAsync(testId)).ReturnsAsync(expectedStatus);

            var service = new StatusEventService(cache, mockRepo.Object);
            var result = await service.GetByIdAsync(testId);

            Assert.Equal(expectedStatus, result);
            mockRepo.Verify(r => r.GetByIdAsync(testId), Times.Once);
            bool existCahe = cache.TryGetValue(new StatusEventCacheKey(testId), out StatusEvent cachedStatus);
            Assert.True(existCahe, "Данные должны были попасть в кэш!");
            Assert.Equal(expectedStatus, cachedStatus);
        }


        [Fact]
        public async Task GetAllAsync_WhenCacheMiss_CallsRepositoryAndSetsCache()
        {
            var mockRepo = new Mock<IStatusEvent>();
            var cache = new MemoryCache(new MemoryCacheOptions());

            var listStatus = new List<StatusEvent>() {
             new StatusEvent(){ Id=1,TypeStatus="Published",Code= "Опубликовано"},
             new StatusEvent(){ Id=2,TypeStatus="Completed",Code= "Завершено"},
             new StatusEvent(){ Id=3,TypeStatus="Draft",Code= "Удалено"},
             new StatusEvent(){ Id=4,TypeStatus="Cancelled",Code= "Отменено"},
             new StatusEvent(){ Id=4,TypeStatus="ReСreation",Code= "В процессе пересоздания"},
        };
            mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(listStatus);

            var service = new StatusEventService(cache, mockRepo.Object);
            var result = await service.GetAllAsync();

            Assert.Equal(listStatus, result);
            mockRepo.Verify(r => r.GetAllAsync(), Times.Once);
            bool existCahe = cache.TryGetValue(CacheConst.allStatusesEvent, out List<StatusEvent> listStatusCache);
            Assert.True(existCahe, "Данные должны были попасть в кэш!");
            Assert.Equal(listStatus, listStatusCache);

        }

    }
}