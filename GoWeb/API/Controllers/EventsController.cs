using GoWeb.Interfaces;
using GoWeb.Models;
using GoWeb.Shared.Models;
using GoWeb.Shared.Requests;
using GoWeb.Shared.Сonstants;
using GoWebApplication.Db.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace GoWeb.API.Controllers
{
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventService eventService;
       

        private readonly IUserEventService userEventService;
        public EventsController(IEventService eventService, IUserEventService userEventService)
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

        [HttpGet(GetUsersFromEventRequest.RouteTemplate)]
        public async Task<ActionResult<GetUsersFromEventRequest.Response>> GetUsersEvent(int id)
        {

            var users = await userEventService.GetUsersEventAsync(id);
            if(users != null)
            {
                return Ok(new GetUsersFromEventRequest.Response(true,null,users));
            }
            return NotFound(new GetUsersFromEventRequest.Response(false, $"Событие с ID {id} не найдено." , new ()));
        }


        [HttpGet(GetEventWithUsersRequest.RouteTemplate)]
        public async Task<ActionResult<GetEventWithUsersRequest.Response>> GetEventWithUsers(int id)
        {
            var eventWitchUsers= await userEventService.GetEventsWithUserAsync(id);
            if (eventWitchUsers != null)
                return Ok(new GetEventWithUsersRequest.Response(true,null,eventWitchUsers));
            return NotFound(new GetEventWithUsersRequest.Response(false, null, new()));

        }


        [HttpGet(GetDataForFilterEventRequest.RouteTemplate)]
        public async Task<ActionResult<GetDataForFilterEventRequest.Response>> GetDataFilter()
        {
            var dataFilter = await eventService.GetDataForFilter();
            if (dataFilter != null)
                return new GetDataForFilterEventRequest.Response(dataFilter);
            return NotFound();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet(JoinEventRequest.RouteTemplate)]
        public async Task<ActionResult<JoinEventRequest.Response>> Join(int id)
        {
            var idUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(idUser))
            {
                return Unauthorized(new JoinEventRequest.Response("Unauthorized","Не удалось определить пользователя или сессия недействительна."));
            }

            var result = await userEventService.JoinAsync(idUser, id);

            return result switch
            {
                JoinResult.SuccessNewRegistration => Ok(new JoinEventRequest.Response("Success", "Вы успешно зарегистрированы на мероприятие!")),
                JoinResult.SuccessStatusUpdated => Ok(new JoinEventRequest.Response("Success", "Вы успешно зарегистрированы на мероприятие!")),
                JoinResult.SuccessInReserve => Ok(new JoinEventRequest.Response("Success", "Места закончились, вы добавлены в резерв.")),

                JoinResult.AlreadyRegistered => BadRequest(new JoinEventRequest.Response("Error", "Вы уже записаны на это мероприятие.")),
                JoinResult.TimeCoincidences => BadRequest(new JoinEventRequest.Response("Error", "Вы уже записаны на другое мероприятие в это же время.")),
                JoinResult.IsufficientlyRequiredRating => BadRequest(new JoinEventRequest.Response("Error", "Недостаточно высокий рейтинг для участия.")),

                JoinResult.UserNotFound => NotFound(new JoinEventRequest.Response("Error", "Пользователь не найден.")),
                JoinResult.EventNotFound => NotFound(new JoinEventRequest.Response("Error", "Мероприятие не найдено.")),

                JoinResult.NoAccessToEvent => NotFound(new JoinEventRequest.Response("Error", "Регистрация на мероприятие недоступна.")),

                _ => StatusCode(500, new JoinEventRequest.Response("Error", "Произошла непредвиденная ошибка."))
            };

        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet(LeaveEventRequest.RouteTemplate)]
        public async Task<ActionResult<LeaveEventRequest.Response>> Leave(int id)
        {
            var idUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(idUser))
            {
                return Unauthorized(new LeaveEventRequest.Response("Unauthorized", "Не удалось определить пользователя или сессия недействительна."));
            }

            var result = await userEventService.LeaveUserAsync(idUser, id);

            return result switch
            {
                LeaveResult.SuccessLeave => Ok(new LeaveEventRequest.Response("Success", "Вы успешно выписались из события.")),

                LeaveResult.AlreadyLeave => BadRequest(new LeaveEventRequest.Response("Error", "Выписка невозможна, вы уже выписаны.")),
                LeaveResult.UserIsNotRegistered => BadRequest(new LeaveEventRequest.Response("Error", "Вы не были зарегистрированы на данное событие.")),
                LeaveResult.EventIsOver => BadRequest(new LeaveEventRequest.Response("Error", "Событие уже завершилось, выписка невозможна.")),
                LeaveResult.EvenWillStartSoon => BadRequest(new LeaveEventRequest.Response("Error", "Событие скоро начнется, выписка невозможна.")),

                LeaveResult.EventNotFound => NotFound(new LeaveEventRequest.Response("Error", "Мероприятие с таким id не найдено.")),
                LeaveResult.UserNotFound => NotFound(new LeaveEventRequest.Response("Error", "Пользователь не найден в системе.")),

                _ => StatusCode(500, new LeaveEventRequest.Response("Error", "Произошла непредвиденная ошибка."))
            };
        }




    }
}
