using GoWeb.Shared.Model;
using GoWeb.Shared.Requests;
using GoWeb.Shared.Service;
using MediatR;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace GoWeb.Shared.Features.Event.Handlers
{
    public class LeaveEventHandler : IRequestHandler<LeaveEventRequest,OperationResult<LeaveEventRequest.Response>>
    {

        private readonly IHttpClientFactory httpClientFactory;
        public LeaveEventHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        public async Task<OperationResult<LeaveEventRequest.Response>> Handle(LeaveEventRequest request, CancellationToken cancellationToken)
        {
           
                var client = httpClientFactory.CreateClient("TokenAPIClient");
                return await client.SafeGetAsJsonAsync<LeaveEventRequest.Response> (LeaveEventRequest.RouteTemplate.Replace("{id}", request.id.ToString()), cancellationToken);
                
        }
    }
}
