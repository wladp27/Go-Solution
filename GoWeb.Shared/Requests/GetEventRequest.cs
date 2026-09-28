using GoWeb.Shared.Model;
using GoWeb.Shared.Models;
using MediatR;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text.Json.Serialization;

namespace GoWeb.Shared.Requests
{
    public record GetEventRequest(int idEvent) : IRequest<OperationResult<GetEventRequest.Response>>
    {
        public const string RouteTemplate = "/api/event/EventSummary/{id}";
        public record Response(EventSummaryDTO EventSummary);
    }
}
