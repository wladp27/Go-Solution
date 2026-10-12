using GoWeb.Shared.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoWeb.Shared.Interfaces
{
    public interface IRequestPut<TModelRequest, TModelResponse> : IRequest<OperationResult<TModelResponse>>
    {
        public TModelRequest Model { get; set; }
        public string RouteTemplate { get; }
    }
    
    
}
