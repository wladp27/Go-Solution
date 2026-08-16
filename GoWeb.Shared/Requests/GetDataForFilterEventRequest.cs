using GoWeb.Shared.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoWeb.Shared.Requests
{
    public record GetDataForFilterEventRequest :IRequest<GetDataForFilterEventRequest.Response>
    {
        public const string RouteTemplate = "/api/Events/filter";
        public record Response(EventFilterDTO filterForEvents);

    }
}
