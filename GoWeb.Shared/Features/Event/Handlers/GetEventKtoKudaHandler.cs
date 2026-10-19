using GoWeb.Shared.Model;
using GoWeb.Shared.Requests;
using GoWeb.Shared.Service;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoWeb.Shared.Features.Event.Handlers
{
    public class GetEventKtoKudaHandler : IRequestHandler<GetEventKtoKudaRequest, OperationResult<GetEventKtoKudaRequest.Response>>
    {
        private readonly IHttpClientFactory httpClientFactory;
        public GetEventKtoKudaHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        public async Task<OperationResult<GetEventKtoKudaRequest.Response>> Handle(GetEventKtoKudaRequest request, CancellationToken cancellationToken)
        {
            var client = httpClientFactory.CreateClient("TokenAPIClient");
            return await client.SafeGetAsJsonAsync<GetEventKtoKudaRequest.Response>(GetEventKtoKudaRequest.RouteTemplate, cancellationToken);
        }
    }
}
