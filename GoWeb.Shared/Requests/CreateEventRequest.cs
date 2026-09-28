using GoWeb.Shared.Interfaces;
using GoWeb.Shared.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace GoWeb.Shared.Requests
{
    public record CreateEventRequest(EventDTO eventCreate) : IRequestPost<EventDTO, CreateEventRequest.Response> 
    {


        public const string Route = "/api/event/create";

        public string RouteTemplate => Route;

        public EventDTO Model { get; set; } = eventCreate;

        public record Response(int idEvent);
    }
}
