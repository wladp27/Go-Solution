using GoWeb.Shared.Model;
using GoWeb.Shared.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoWeb.Shared.Requests
{
    public record CreateLocationRequest(LocationCreateDTO Location): IRequest<CreateLocationRequest.Response>
    {
        public const string RouteTemplate = "/api/location/create";
        public class Response : OperationResult<int>
        {
            protected Response(bool isSuccess, int value, string errorMessage)
                : base(isSuccess, value, errorMessage) { }

            public new static Response Success(int value) =>
                new(true, value, string.Empty);

            public new static Response Failure(string errorMessage) =>
                new(false, default!, errorMessage);
        }
    }
}
