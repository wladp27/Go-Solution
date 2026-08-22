using GoWeb.Shared.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoWeb.Shared.Requests
{
   public record GetAllTypesEventsRequest: IRequest<GetAllTypesEventsRequest.Response>
    {
        public const string RouteTemplate = "/api/TypesEvents";
        public record Response(bool IsSuccess, string? Message, List<EventTypeDTO> Types);
    }
}
