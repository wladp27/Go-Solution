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
            var eventView = mapper.Map<List<EventSummaryDTO>>(listEvent);
            if (eventView != null)
            {
                return new GetMyEventsRequest.Response(eventView);
            }
            return NotFound(new { message = $"Ошибка сервера" });
        }
    }
}
