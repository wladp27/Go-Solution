using GoWeb.Interfaces;
using GoWeb.Models;
using GoWeb.Shared.Models;
using GoWeb.Shared.Requests;
using Microsoft.AspNetCore.Mvc;


namespace GoWeb.API.Controllers
{
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventService eventService;
       

        private readonly IUserEventService userEventService;
        public EventsController(IEventService eventService, IUserEventService userEventService, IUserEvent userEvent)
        {
            this.eventService = eventService;
            this.userEventService = userEventService;
           
        }

        [HttpPost]
        [Route(GetPublishedEventsRequest.RouteTemplate)]
        public async Task<ActionResult<GetPublishedEventsRequest.Response>> GetPublishedEvents([FromBody] EventFilterDTO filter)
        {
            var listEvent = await eventService.GetFilteredEventsAsync(filter);
            if(listEvent!=null)
                return new GetPublishedEventsRequest.Response(listEvent);
            return new GetPublishedEventsRequest.Response(new ());
        }

        [HttpGet("[controller]/[action]/{id}")]
        public async Task<ActionResult<EventWithUsersDTO>> Event(int id)
        {
            var ev = await userEventService.GetEventsWithUserAsync(id);
            if (ev != null)
            {
                return ev;
            }
            return NotFound(); 
        }

        [HttpGet(GetEventRequest.RouteTemplate)]
        public async Task<ActionResult<GetEventRequest.Response>> GetEventSummary(int id)
        {
            var ev = await eventService.GetEventByIdAsync(id);
            if (ev != null)
            {
                return new GetEventRequest.Response(ev);
            }
            return NotFound(new { message = $"Событие с ID {id} не найдено." });
        }


        [HttpGet(GetDataForFilterEventRequest.RouteTemplate)]
        public async Task<ActionResult<GetDataForFilterEventRequest.Response>> GetDataFilter()
        {
            var dataFilter = await eventService.GetDataForFilter();
            if (dataFilter != null)
                return new GetDataForFilterEventRequest.Response(dataFilter);
            return NotFound();
        }


    }
}
