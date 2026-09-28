using AutoMapper;
using GoWeb.Interfaces;
using GoWeb.Repositories;
using GoWeb.Shared.Model;
using GoWeb.Shared.Requests;
using GoWebApplication.Db.Models;
using Microsoft.AspNetCore.Mvc;

namespace GoWeb.API.Controllers
{
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly ILocationRepository locationRepository;
        private readonly IMapper mapper;
        public LocationController(ILocationRepository locationRepository, IMapper mapper)
        {
            this.locationRepository = locationRepository;
            this.mapper = mapper;
        }
        
        [HttpPost(CreateLocationRequest.Route)]
         public async Task<ActionResult<CreateLocationRequest.Response>> Create([FromBody] LocationCreateDTO location)
         {
            var locationDb = mapper.Map<LocationCreateDTO, Location>(location);

            var result = await locationRepository.AddAsync(locationDb);
            if(result.IsSuccess)
            {
                return Ok(new CreateLocationRequest.Response(result.Data));
            }
            return Problem(detail: result.ErrorMessage, statusCode: 400);
         }
        
        [HttpGet(GetFilteredLocationsRequest.RouteTemplate)]
        public async Task<ActionResult<GetFilteredLocationsRequest.Response>> GetLocations(string address, int idCity)
        {
            var result = await locationRepository.GetPreviewLocations(address, idCity);
            if (result.IsSuccess)
            {
                return Ok(new GetFilteredLocationsRequest.Response(result.Data!));
            }
            return Problem(
                            detail: "Локация с указанным адресом не найдена",
                            statusCode: StatusCodes.Status404NotFound,
                            title: "Not Found"
                           );
        }

    }
}
