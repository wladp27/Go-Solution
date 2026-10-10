using GoWeb.Shared.Model;
using MediatR;


namespace GoWeb.Shared.Requests
{
    public record LeaveEventRequest(int id) : IRequest<OperationResult<LeaveEventRequest.Response>>
    {
        public const string RouteTemplate = "/api/event/{id}/leave";
        public record Response(string Message);
    }
}
