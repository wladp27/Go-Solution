using GoWeb.Interfaces;
using GoWeb.Service;
using GoWeb.Shared.Model;
using GoWeb.Shared.Requests;
using Microsoft.AspNetCore.Mvc;

namespace GoWeb.API.Controllers
{
    public class ManageController : ControllerBase
    {
        public readonly IEventService eventService;
        public ManageController(IEventService eventService) 
        {
            this.eventService = eventService;
        }


        //проверки на существование всего сделать от локации до типа события
        [HttpPost(CreateEventRequest.RouteTemplate)]
        public async Task<ActionResult<CreateEventRequest.Response>> CreateEvent([FromBody] EventDTO eventCreate)
        {
           var idEvent = await eventService.AddAsync(eventCreate);
           return Ok(CreateEventRequest.Response.Success(idEvent));
        }


        [HttpPut(EditEventRequest.RouteTemplate)]
        public async Task<ActionResult<EditEventRequest.Response>> EditEvent([FromBody] EventDTO eventEdit)
        {
           await eventService.UpdateAsync(eventEdit);
           return Ok(EditEventRequest.Response.Success());
        }


    }
}
