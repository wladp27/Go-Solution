using GoWeb.Shared.Interfaces;
using GoWeb.Shared.Model;
using GoWeb.Shared.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoWeb.Shared.Requests
{
    public record LoginRequest(UserLoginDTO userLoginDTO) : IRequestPost<UserLoginDTO, LoginRequest.Response>
    {
        public const string Route = "/api/auth/login";

        public string RouteTemplate => Route;

        public UserLoginDTO Model { get; set; } = userLoginDTO;

        public record Response(TokenDTO Token);
    }

}
