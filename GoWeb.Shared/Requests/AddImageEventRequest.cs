using GoWeb.Shared.Interfaces;
using GoWeb.Shared.Model;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace GoWeb.Shared.Requests
{
    public record AddImageEventRequest(IBrowserFile Image) : IRequestPost<IBrowserFile, AddImageEventRequest.Response>
    {
        public const string Route = "/api/images/add";

        public IBrowserFile Model { get ; set ; } = Image;

        public string RouteTemplate => Route;

        public record Response(string ImagePath);
    }
}
