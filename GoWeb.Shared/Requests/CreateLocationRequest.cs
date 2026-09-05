using GoWeb.Shared.Model;
using GoWeb.Shared.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace GoWeb.Shared.Requests
{
    public record CreateLocationRequest(LocationCreateDTO Location) : IRequest<CreateLocationRequest.Response>
    {
        public const string RouteTemplate = "/api/location/create";

        public class Response : OperationResult<int>
        {
            
            public Response(bool isSuccess, int data, string errorMessage)
                : base(isSuccess, data, errorMessage) { }

            public new static Response Success(int value) =>
                new(true, value, string.Empty);

            public new static Response Failure(string errorMessage) =>
                new(false, default!, errorMessage);
        }
    }
}
