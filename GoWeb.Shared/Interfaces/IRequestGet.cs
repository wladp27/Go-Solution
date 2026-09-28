using GoWeb.Shared.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoWeb.Shared.Interfaces
{
    internal interface IRequestGet<TModelResponse> : IRequest<OperationResult<TModelResponse>>
    {
        public string RouteTemplate { get; }
    }
}
