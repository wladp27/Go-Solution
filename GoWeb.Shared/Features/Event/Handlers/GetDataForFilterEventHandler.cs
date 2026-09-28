using MediatR;
using GoWeb.Shared.Requests;
using System.Net.Http.Json;
using GoWeb.Shared.Models;
using GoWeb.Shared.Model;
using GoWeb.Shared.Service;

namespace GoWeb.Shared.Features.Event.Handlers
{
    public class GetDataForFilterEventHandler : IRequestHandler<GetDataForFilterEventRequest,OperationResult<GetDataForFilterEventRequest.Response>>
    {
        public HttpClient _httpClient { get; set; }
        public GetDataForFilterEventHandler(HttpClient httpClient) 
        {
            _httpClient= httpClient;
        }

        public async Task<OperationResult<GetDataForFilterEventRequest.Response>> Handle(GetDataForFilterEventRequest request, CancellationToken cancellationToken)
        {
         
                return await _httpClient.SafeGetAsJsonAsync<GetDataForFilterEventRequest.Response>(GetDataForFilterEventRequest.RouteTemplate, cancellationToken);
        }

   
    }
}
