using GoWeb.Models;
using GoWebApplication.Db.Models;
using GoWeb.Shared.Models;
namespace GoWeb.Interfaces
{
    public interface ICityService
    {
        public Task<CityDTO?> GetByIdAsync(int id);
        public Task<bool> AddAsync(CityDTO city);
        public Task<bool> Update(CityDTO city);
        public Task<bool> DeleteAsync(CityDTO city);
        public Task<List<CityDTO>?> GetAllAsync();

    }
}
