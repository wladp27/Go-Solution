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
    public record GetCitiesRequest: IRequest<OperationResult<GetCitiesRequest.Response>>
    {
        public const string RouteTemplate = "/api/Cities";
        public record Response(List<CityDTO> Cities);
    }
}
