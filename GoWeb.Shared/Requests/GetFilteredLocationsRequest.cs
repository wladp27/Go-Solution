using GoWeb.Shared.Model;
using GoWeb.Shared.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoWeb.Shared.Requests
{
    public record GetFilteredLocationsRequest(string Address, int IdCity) : IRequest<GetFilteredLocationsRequest.Response>
    {
        public const string RouteTemplate = "/api/location/{idCity}/{address}";
        public class Response : OperationResult<List<LocationPreviewDTO>>
        {
            protected Response(bool isSuccess, List<LocationPreviewDTO> value, string errorMessage)
                : base(isSuccess, value, errorMessage) { }

            public new static Response Success(List<LocationPreviewDTO> value) =>
                new(true, value, string.Empty);

            public new static Response Failure(string errorMessage) =>
                new(false, default!, errorMessage);
        }

    }
}
