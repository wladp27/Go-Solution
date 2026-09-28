using GoWeb.Shared.Model;
using GoWeb.Shared.Requests;
using GoWeb.Shared.Service;
using MediatR;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace GoWeb.Shared.Features.Manage.Handlers
{
    public class GetAllTypesEventsHandler : IRequestHandler<GetAllTypesEventsRequest,OperationResult<GetAllTypesEventsRequest.Response>>
    {
        private IHttpClientFactory httpClientFactory;
        public GetAllTypesEventsHandler(IHttpClientFactory httpClientFactory) 
        { 
            this.httpClientFactory = httpClientFactory;
        }
        public async Task<OperationResult<GetAllTypesEventsRequest.Response>> Handle(GetAllTypesEventsRequest request, CancellationToken cancellationToken)
        {
            var client = httpClientFactory.CreateClient("TokenAPIClient");
            return await client.SafeGetAsJsonAsync<GetAllTypesEventsRequest.Response>(GetAllTypesEventsRequest.RouteTemplate, cancellationToken);
        }
    }
}
