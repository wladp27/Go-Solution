using GoWeb.Interfaces;
using GoWeb.Shared.Models;
using GoWeb.Shared.Requests;
using Microsoft.AspNetCore.Mvc;

namespace GoWeb.API.Controllers
{
    public class TypeEventController : ControllerBase
    {
        private readonly IEventService eventService;
        private readonly IEventTypeService eventTypeService;
        public TypeEventController(IEventService eventService, IEventTypeService eventTypeService)
        {
            this.eventService = eventService;
            this.eventTypeService = eventTypeService;
        }

        [HttpGet(GetTypeEventsForCityRequest.RouteTemplate)]
        public async Task<ActionResult<GetTypeEventsForCityRequest.Response>> GetTypesEvents(int idCity)
        {
            var listTypes =  await eventService.GetTypesEventsForCity(idCity);
            if (listTypes!=null)
                return new GetTypeEventsForCityRequest.Response(listTypes);
            return NotFound();
        }

        [HttpGet(GetAllTypesEventsRequest.RouteTemplate)]
        public async Task<ActionResult<GetAllTypesEventsRequest.Response>>GetAllTypes()
        {
            var listTypes = await eventTypeService.GetAllAsync();
            if (listTypes != null)
                return new GetAllTypesEventsRequest.Response(true,null,listTypes);
            return NotFound(new GetAllTypesEventsRequest.Response(false, "Types not found", new()));
        }

    }
}
