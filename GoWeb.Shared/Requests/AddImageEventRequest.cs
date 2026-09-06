using GoWeb.Shared.Model;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace GoWeb.Shared.Requests
{
    public record AddImageEventRequest(IBrowserFile Image) : IRequest<AddImageEventRequest.Response>
    {
        public const string RouteTemplate = "/api/images/add";
        public class Response : OperationResult<string>
        {
            [JsonConstructor]
            public Response(bool isSuccess, string data, string errorMessage)
                : base(isSuccess, data, errorMessage) { }

            public new static Response Success(string value) =>
                new(true, value, string.Empty);

            public new static Response Failure(string errorMessage) =>
                new(false, default!, errorMessage);
        }
    }
}
