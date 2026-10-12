using GoWeb.Shared.Interfaces;
using GoWeb.Shared.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoWeb.Shared.Requests
{
    public record EditEventRequest(EventDTO eventEdit) : IRequestPut<EventDTO,EditEventRequest.Response>
    {
        public const string Route = "/api/events/{id}";

        public EventDTO Model { get ; set; } = eventEdit;

        public string RouteTemplate => Route;

        public record Response();

    }
}

