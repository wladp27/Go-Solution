using GoWeb.Service;
using GoWeb.Shared.Requests;
using Microsoft.AspNetCore.Mvc;

namespace GoWeb.Shared.Service
{
    [ApiController]
    public class KtoKudaController : ControllerBase
    {
        private readonly KtoKudaService ktoKudaService;

        public KtoKudaController(KtoKudaService ktoKudaService)
        {
            this.ktoKudaService = ktoKudaService;
        }

        [HttpGet(GetEventKtoKudaRequest.RouteTemplate)]
        public async Task<ActionResult<GetEventKtoKudaRequest.Response>> GetEvents()
        {
            var listEvent = await ktoKudaService.GetEvent();
            return Ok(new GetEventKtoKudaRequest.Response(listEvent));
        }
    }
}
