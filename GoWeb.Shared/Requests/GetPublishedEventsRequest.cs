using GoWeb.Shared.Interfaces;
using GoWeb.Shared.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoWeb.Shared.Requests
{
    public record GetPublishedEventsRequest(EventFilterDTO filter) : IRequestPost<EventFilterDTO, GetPublishedEventsRequest.Response>
    {
        public const string Route = "/api/events/published";

        public string RouteTemplate => Route;

        public EventFilterDTO Model { get; set; } = filter;

        public record Response(List<EventSummaryDTO> EventsSummary);
    }
}
