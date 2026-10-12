using AutoMapper;
using GoWeb.Interfaces;
using GoWeb.Models;
using GoWeb.Shared.Сonstants;
using GoWebApplication.Db.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using GoWeb.Shared.Models;
using GoWeb.Shared.Requests;
namespace GoWeb.API.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserController : ControllerBase
    {
        private readonly IUserEventService userEvent;
        private readonly IMapper mapper;
        public UserController(IUserEventService userEvent, IMapper mapper)
        {
            this.userEvent = userEvent;
            this.mapper = mapper;
        }

     
   

   

        [Route(GetMyEventsRequest.RouteTemplate)]
        public async Task<ActionResult<GetMyEventsRequest.Response>> MyEvents()
        {
            var listEvent = await userEvent.GetAllAttendedEventsUserAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var eventsView = mapper.Map<List<EventSummaryDTO>>(listEvent);
            if (eventsView != null)
            {
                return Ok(new GetMyEventsRequest.Response(eventsView));
            }
            return Problem(
                           detail: "Не удалось получить список событий",
                           statusCode: StatusCodes.Status404NotFound,
                           title: "Not Found");
        }
    }
}
