using AutoMapper;
using GoWeb.Interfaces;
using GoWeb.Models;
using GoWeb.Сonstants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using GoWeb.Shared.Models;

namespace GoWeb.Areas.User.Controllers
{
    [Area("User")]
    public class EventController : Controller
    {
        private readonly IUserEventService userEvent;
        private readonly IMapper mapper;

        public EventController(IUserEventService userEvent, IMapper mapper)
        {
            this.mapper = mapper;
            this.userEvent = userEvent;
        }

        

      

        [Authorize]
        public async Task<IActionResult> MyEvents()
        {
            var listEvent = await  userEvent.GetAllAttendedEventsUserAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var eventView = mapper.Map<List<EventSummaryDTO>>(listEvent);
            return View(eventView);
        }

    }
}
