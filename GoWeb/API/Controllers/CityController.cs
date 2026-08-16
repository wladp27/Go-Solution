using GoWeb.Interfaces;
using GoWeb.Shared.Requests;
using Microsoft.AspNetCore.Mvc;

namespace GoWeb.API.Controllers
{
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly ICityService cityService;
        public CityController(ICityService cityService) 
        {
            this.cityService = cityService;
        }

        [HttpGet(GetCitiesRequest.RouteTemplate)]
        public async Task<ActionResult<GetCitiesRequest.Response>> GetCities()
        {
            var cities = await cityService.GetAllAsync();
            if (cities != null)
            {
                return Ok(new GetCitiesRequest.Response(cities));
            }
            return NotFound();
        }


    }
}
