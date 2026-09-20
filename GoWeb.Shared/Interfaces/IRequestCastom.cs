using GoWeb.Shared.Model;
using GoWeb.Shared.Requests;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoWeb.Shared.Interfaces
{
    public interface IRequestCastom<TModel,TData,out TResponse> : IRequest<TResponse>
            where TResponse : OperationResult<TData>
    {
        public TModel Model { get; set; }
        public string RouteTemplate { get; }
    }
}
