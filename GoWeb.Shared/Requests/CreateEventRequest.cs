using GoWeb.Shared.Interfaces;
using GoWeb.Shared.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace GoWeb.Shared.Requests
{
    public record CreateEventRequest(EventDTO eventCreate) : IRequestCastom<EventDTO, int, CreateEventRequest.Response>
    {


        public const string Route = "/api/event/create";

        public string RouteTemplate => Route;

        public EventDTO Model { get; set; } = eventCreate;


        public class Response : OperationResult<int>
        {
            public int IdEvent { get; set; }
            [JsonConstructor]
            public Response(bool isSuccess,  int Data, string errorMessage): base(isSuccess, Data, errorMessage) { }
            public static Response CastOperation(OperationResult<int> operationResult)
            {
                if(operationResult.IsSuccess)
                {
                    var response = new Response(operationResult.IsSuccess, operationResult.Data, operationResult.ErrorMessage);
                    response.IdEvent = operationResult.Data;
                    return response;
                }
                return new(false, default!, operationResult.ErrorMessage);
            }
        }

    }
}
