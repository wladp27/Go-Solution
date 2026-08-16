using GoWeb.Interfaces;
using GoWeb.Shared.Models;
using GoWeb.Shared.Requests;
using Microsoft.AspNetCore.Mvc;

namespace GoWeb.API.Controllers
{
    public class TypeEventController : ControllerBase
    {
        private readonly IEventService eventService;
        public TypeEventController(IEventService eventService)
        {
            this.eventService = eventService;
        }

        [HttpGet(GetTypeEventsForCityRequest.RouteTemplate)]
        public async Task<ActionResult<GetTypeEventsForCityRequest.Response>> GetTypesEvents(int idCity)
        {
            var listTypes =  await eventService.GetTypesEventsForCity(idCity);
            if (listTypes!=null)
                return new GetTypeEventsForCityRequest.Response(listTypes);
            return NotFound();
        }
    }
}
