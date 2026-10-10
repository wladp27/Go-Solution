using GoWeb.Interfaces;
using GoWeb.Models;
using GoWeb.Service;
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
        [Route(GetPublishedEventsRequest.Route)]
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
                return Ok(new GetEventRequest.Response(ev));
            }
            return Problem(
                            detail: "Событие с указанным ID не найдено",
                            statusCode: StatusCodes.Status404NotFound,
                            title: "Not Found");
        }

        [HttpGet(GetUsersFromEventRequest.RouteTemplate)]
        public async Task<ActionResult<GetUsersFromEventRequest.Response>> GetUsersEvent(int id)
        {

            var users = await userEventService.GetUsersEventAsync(id);
            if(users != null)
            {
                return Ok(new GetUsersFromEventRequest.Response(users));
            }
            return Problem( detail: "Не удалось загрузить пользователей события",
                            statusCode: StatusCodes.Status404NotFound,
                            title: "Not Found");
        }


        [HttpGet(GetEventWithUsersRequest.RouteTemplate)]
        public async Task<ActionResult<GetEventWithUsersRequest.Response>> GetEventWithUsers(int id)
        {
            var eventWitchUsers= await userEventService.GetEventsWithUserAsync(id);
            if (eventWitchUsers != null)
                return Ok(new GetEventWithUsersRequest.Response(eventWitchUsers));
            return Problem(detail: "Не удалось загрузить пользователей события с ID {id} ",
                            statusCode: StatusCodes.Status404NotFound,
                            title: "Not Found");
        }


        [HttpGet(GetDataForFilterEventRequest.RouteTemplate)]
        public async Task<ActionResult<GetDataForFilterEventRequest.Response>> GetDataFilter()
        {
            var dataFilter = await eventService.GetDataForFilter();
            if (dataFilter != null)
                return new GetDataForFilterEventRequest.Response(dataFilter);
            return Problem(
                            detail: "Получить данные для фильтра не удалось",
                            statusCode: StatusCodes.Status404NotFound,
                            title: "Not Found");
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet(JoinEventRequest.RouteTemplate)]
        public async Task<ActionResult<JoinEventRequest.Response>> Join(int id)
        {
            var idUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(idUser))
            {
                return Problem(
                    detail: "Не удалось определить пользователя или сессия недействительна.",
                    statusCode: StatusCodes.Status401Unauthorized
                );
            }

            var result = await userEventService.JoinAsync(idUser, id);

            return result switch
            {
                JoinResult.SuccessNewRegistration or JoinResult.SuccessStatusUpdated =>
                    Ok(new JoinEventRequest.Response("Вы успешно зарегистрированы на мероприятие!")),

                JoinResult.SuccessInReserve =>
                    Ok(new JoinEventRequest.Response("Места закончились, вы добавлены в резерв.")),

                JoinResult.AlreadyRegistered => Problem(
                    detail: "Вы уже записаны на это мероприятие.",
                    statusCode: StatusCodes.Status400BadRequest),

                JoinResult.TimeCoincidences => Problem(
                    detail: "Вы уже записаны на другое мероприятие в это же время.",
                    statusCode: StatusCodes.Status400BadRequest),

                JoinResult.IsufficientlyRequiredRating => Problem(
                    detail: "Недостаточно высокий рейтинг для участия.",
                    statusCode: StatusCodes.Status400BadRequest),

                JoinResult.UserNotFound => Problem(
                    detail: "Пользователь не найден.",
                    statusCode: StatusCodes.Status404NotFound),

                JoinResult.EventNotFound => Problem(
                    detail: "Мероприятие не найдено.",
                    statusCode: StatusCodes.Status404NotFound),

                JoinResult.NoAccessToEvent => Problem(
                    detail: "Регистрация на мероприятие недоступна.",
                    statusCode: StatusCodes.Status403Forbidden), // Для отсутствия доступа семантически лучше 403 Forbidden

                _ => Problem(
                    detail: "Произошла непредвиденная ошибка при записи.",
                    statusCode: StatusCodes.Status500InternalServerError)
            };

        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet(LeaveEventRequest.RouteTemplate)]
        public async Task<ActionResult<LeaveEventRequest.Response>> Leave(int id)
        {
            var idUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(idUser))
            {
                return Problem(
                    detail: "Не удалось определить пользователя или сессия недействительна.",
                    statusCode: StatusCodes.Status401Unauthorized
                );
            }

            var result = await userEventService.LeaveUserAsync(idUser, id);

            return result switch
            {
                LeaveResult.SuccessLeave => Ok(new LeaveEventRequest.Response("Вы успешно выписаны из мероприятия")),

                LeaveResult.AlreadyLeave => Problem(
                    detail: "Выписка невозможна, вы уже выписаны.",
                    statusCode: StatusCodes.Status400BadRequest),

                LeaveResult.UserIsNotRegistered => Problem(
                    detail: "Вы не были зарегистрированы на данное событие.",
                    statusCode: StatusCodes.Status400BadRequest),

                LeaveResult.EventIsOver => Problem(
                    detail: "Событие уже завершилось, выписка невозможна.",
                    statusCode: StatusCodes.Status400BadRequest),

                LeaveResult.EvenWillStartSoon => Problem(
                    detail: "Событие скоро начнется, выписка невозможна.",
                    statusCode: StatusCodes.Status400BadRequest),

                LeaveResult.EventNotFound => Problem(
                    detail: "Мероприятие с таким id не найдено.",
                    statusCode: StatusCodes.Status404NotFound),

                LeaveResult.UserNotFound => Problem(
                    detail: "Пользователь не найден в системе.",
                    statusCode: StatusCodes.Status404NotFound),

                _ => Problem(
                    detail: "Произошла непредвиденная ошибка при выписке.",
                    statusCode: StatusCodes.Status500InternalServerError)
            };
        }




    }
}
