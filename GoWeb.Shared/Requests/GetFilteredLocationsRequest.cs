using GoWeb.Shared.Model;
using GoWeb.Shared.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace GoWeb.Shared.Requests
{
    public record GetFilteredLocationsRequest(string Address, int IdCity) : IRequest<OperationResult<GetFilteredLocationsRequest.Response>>
    {
        public const string RouteTemplate = "/api/location/{idCity}/{address}";
        public record Response(List<LocationPreviewDTO> LocationPreviews);
    }
}
