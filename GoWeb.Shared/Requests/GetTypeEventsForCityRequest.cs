using GoWeb.Shared.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoWeb.Shared.Requests
{
    public record GetTypeEventsForCityRequest(int idCity):IRequest<GetTypeEventsForCityRequest.Response>
    {
        public const string RouteTemplate = "/api/type-events/{idCity}";
        public record Response(List<EventTypeDTO> typesEvents);
    }
}
