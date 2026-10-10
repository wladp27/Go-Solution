using GoWeb.Shared.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoWeb.Shared.Requests
{
    public record JoinEventRequest(int id) : IRequest<OperationResult<JoinEventRequest.Response>>
    {
        public const string RouteTemplate = "/api/event/{id}/join";
        public record Response(string Message);
    }
}
