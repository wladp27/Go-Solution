using GoWeb.Shared.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoWeb.Shared.Requests
{
    public record DeleteImageEventRequest(string nameImage) : IRequest<DeleteImageEventRequest.Response>
    {
        public const string RouteTemplate = "/api/images/{nameImage}";
        public class Response : OperationResult
        {
            public Response(bool isSuccess, string errorMessage) : base(isSuccess, errorMessage) { }
            public new static Response Success() => new(true, string.Empty);
            public new static Response Failure(string errorMessage) => new(false, errorMessage);
        }

    }
}
