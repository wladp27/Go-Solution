using GoWeb.Shared.Interfaces;
using GoWeb.Shared.Model;
using GoWeb.Shared.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace GoWeb.Shared.Requests
{
    public record CreateLocationRequest(LocationCreateDTO Location) : IRequestPost<LocationCreateDTO, CreateLocationRequest.Response>
    {
        public const string Route = "/api/location/create";

        public string RouteTemplate => Route;

        public LocationCreateDTO Model { get; set; } = Location;

        public record Response(int idLocation);
    }
}
