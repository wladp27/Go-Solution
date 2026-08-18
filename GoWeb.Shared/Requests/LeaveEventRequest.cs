using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoWeb.Shared.Requests
{
    public record LeaveEventRequest(int id) : IRequest<LeaveEventRequest.Response>
    {
        public const string RouteTemplate = "/api/event/{id}/leave";
        public record Response(string Code, string Message) : BaseResponse(Code, Message);
    }
}
