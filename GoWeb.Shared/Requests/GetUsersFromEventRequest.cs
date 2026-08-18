using GoWeb.Shared.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoWeb.Shared.Requests
{
    public record GetUsersFromEventRequest(int idEvent) : IRequest<GetUsersFromEventRequest.Response>
    {
        public const string RouteTemplate = "/api/event/{id}/users";
        public record Response(bool IsSuccess,string? Message, List<UserPrewievDTO> users);

    }
}
