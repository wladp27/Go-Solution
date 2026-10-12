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
    public record GetMyEventsRequest : IRequest<OperationResult<GetMyEventsRequest.Response>>
    {
        public const string RouteTemplate = "/api/Profile/MyEvents";
        public record Response(List<EventSummaryDTO> eventSummary);

    }
}
