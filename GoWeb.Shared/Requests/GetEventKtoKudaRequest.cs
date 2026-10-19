using GoWeb.Shared.Model;
using GoWeb.Shared.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoWeb.Shared.Requests
{
    public record GetEventKtoKudaRequest() :IRequest<OperationResult<GetEventKtoKudaRequest.Response>>
    {
        public const string RouteTemplate = "/api/KtoKuda/events";
        public record Response(List<EventSummaryDTO> Events);
    }
}
