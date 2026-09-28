using GoWeb.Shared.Model;
using GoWeb.Shared.Requests;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoWeb.Shared.Interfaces
{
    public interface IRequestPost<TModelRequest,TModelResponse> : IRequest<OperationResult<TModelResponse>>
    {
        public TModelRequest Model { get; set; }
        public string RouteTemplate { get; }
    }
}
