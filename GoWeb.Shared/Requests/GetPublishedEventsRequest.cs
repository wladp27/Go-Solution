using GoWeb.Shared.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoWeb.Shared.Requests
{
        public record GetPublishedEventsRequest(EventFilterDTO filter) : IRequest<GetPublishedEventsRequest.Response>
        {
            public const string RouteTemplate = "/api/events/published";
            public record Response(List<EventSummaryDTO> eventSummary);
        }
}
