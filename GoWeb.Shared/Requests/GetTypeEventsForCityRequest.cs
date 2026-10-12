using GoWeb.Shared.Model;
using GoWeb.Shared.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoWeb.Shared.Requests
{
    public record GetTypeEventsForCityRequest(int idCity):IRequest<OperationResult<GetTypeEventsForCityRequest.Response>>
    {
        public const string RouteTemplate = "/api/city/type-events/{id}";
        public record Response(List<EventTypeDTO> typesEvents);
    }
}
