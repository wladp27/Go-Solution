using GoWeb.Shared.Model;
using GoWeb.Shared.Models;
using MediatR;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text.Json.Serialization;

namespace GoWeb.Shared.Requests
{
    public record GetEventRequest(int idEvent) : IRequest<GetEventRequest.Response>
    {
        public const string RouteTemplate = "/api/event/EventSummary/{id}";
        public class Response : OperationResult<EventSummaryDTO>
        {
            [JsonConstructor]
            public Response(bool isSuccess, EventSummaryDTO data, string errorMessage) : base(isSuccess, data, errorMessage) { }
            public EventSummaryDTO EventSummary => Data!;
            public new static Response Success(EventSummaryDTO value) => new(true, value, string.Empty);
            public new static Response Failure(string errorMessage) => new(false, default!, errorMessage);
        }
    }
}
