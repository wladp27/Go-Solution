using GoWeb.Shared.Model;
using GoWeb.Shared.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace GoWeb.Shared.Requests
{
    public record GetFilteredLocationsRequest(string Address, int IdCity) : IRequest<GetFilteredLocationsRequest.Response>
    {
        public const string RouteTemplate = "/api/location/{idCity}/{address}";
        public class Response : OperationResult<List<LocationPreviewDTO>>
        {
            [JsonConstructor]
            public Response(bool isSuccess, List<LocationPreviewDTO> data, string errorMessage)
                : base(isSuccess, data, errorMessage) { }

            public new static Response Success(List<LocationPreviewDTO> value) =>
                new(true, value, string.Empty);

            public new static Response Failure(string errorMessage) =>
                new(false, default!, errorMessage);
        }

    }
}
