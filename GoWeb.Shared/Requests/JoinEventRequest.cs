using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoWeb.Shared.Requests
{
    public record JoinEventRequest(int id) : IRequest<JoinEventRequest.Response>
    {
        public const string RouteTemplate = "/api/event/{id}/join";
        public record Response(string Code, string Message): BaseResponse(Code, Message); 
    }
}
