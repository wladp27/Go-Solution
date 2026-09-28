using GoWeb.Shared.Model;
using GoWeb.Shared.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoWeb.Shared.Requests
{
    public record class GetEventWithUsersRequest(int Id) :IRequest<OperationResult<GetEventWithUsersRequest.Response>>
    {

        public const string RouteTemplate = "/api/event/with-users/{id}";
        public record Response(EventWithUsersDTO EventWithUsersDTO);
    }
}
