using GoWeb.Shared.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace GoWeb.Shared.Requests
{
    public  record CreateEventRequest(EventDTO eventCreate) : IRequest<CreateEventRequest.Response>
    {

        public const string RouteTemplate = "/api/event/create";


        public class Response : OperationResult<int>
        {
            [JsonConstructor]
            public Response(bool isSuccess,  int data, string errorMessage): base(isSuccess, data, errorMessage) { }
            public int EventId => Data;
            public new static Response Success(int value) => new(true, value, string.Empty);
            public new static Response Failure(string errorMessage) => new(false, default!, errorMessage);
        }

    }
}
