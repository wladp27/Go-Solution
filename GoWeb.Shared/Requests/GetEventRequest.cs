using GoWeb.Shared.Models;
using MediatR;


namespace GoWeb.Shared.Requests
{
    public record GetEventRequest(int idEvent) : IRequest<GetEventRequest.Response>
    {
        public const string RouteTemplate = "/api/Events/EventSummary/{id}";
        public record Response(EventSummaryDTO eventSummary);

    }
}
