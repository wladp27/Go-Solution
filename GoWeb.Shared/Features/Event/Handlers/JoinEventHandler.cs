using GoWeb.Shared.Model;
using GoWeb.Shared.Requests;
using GoWeb.Shared.Service;
using MediatR;
using Microsoft.VisualBasic;
using System.Net.Http.Json;


namespace GoWeb.Shared.Features.Event.Handlers
{
    public class JoinEventHandler : IRequestHandler<JoinEventRequest, OperationResult<JoinEventRequest.Response>>
    {

        private readonly IHttpClientFactory httpClientFactory;
        public JoinEventHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        public async Task<OperationResult<JoinEventRequest.Response>> Handle(JoinEventRequest request, CancellationToken cancellationToken)
        {
           var client = httpClientFactory.CreateClient("TokenAPIClient");
           return await client.SafeGetAsJsonAsync<JoinEventRequest.Response>(JoinEventRequest.RouteTemplate.Replace("{id}", request.id.ToString()),cancellationToken);
                
        }
    }
}
