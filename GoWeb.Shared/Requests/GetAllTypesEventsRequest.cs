using GoWeb.Shared.Model;
using GoWeb.Shared.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoWeb.Shared.Requests
{
   public record GetAllTypesEventsRequest: IRequest<OperationResult<GetAllTypesEventsRequest.Response>>
    {
        public const string RouteTemplate = "/api/TypesEvents";
        public record Response(List<EventTypeDTO> Types);
    }
}
