using GoWeb.Shared.Model;
using GoWeb.Shared.Requests;
using GoWeb.Shared.Service;
using MediatR;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;

namespace GoWeb.Shared.Features.Event.Handlers
{
    public class GetMyEventsHandler : IRequestHandler<GetMyEventsRequest,OperationResult<GetMyEventsRequest.Response>>
    {

        private readonly IHttpClientFactory httpClientFactory;
        public GetMyEventsHandler(IHttpClientFactory httpClientFactory) 
        {
            this.httpClientFactory = httpClientFactory;


        }
        public async Task<OperationResult<GetMyEventsRequest.Response>> Handle(GetMyEventsRequest request, CancellationToken cancellationToken)
        {
          
                var client = httpClientFactory.CreateClient("TokenAPIClient");
                return await client.SafeGetAsJsonAsync<GetMyEventsRequest.Response>(GetMyEventsRequest.RouteTemplate,cancellationToken);
        }
    }
}
