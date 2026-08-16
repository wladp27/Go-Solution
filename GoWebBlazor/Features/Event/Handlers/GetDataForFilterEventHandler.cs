using MediatR;
using GoWeb.Shared.Requests;
using System.Net.Http.Json;
using GoWeb.Shared.Models;

namespace GoWebBlazor.Features.Event.Handlers
{
    public class GetDataForFilterEventHandler : IRequestHandler<GetDataForFilterEventRequest, GetDataForFilterEventRequest.Response>
    {
        public HttpClient _httpClient { get; set; }
        public GetDataForFilterEventHandler(HttpClient httpClient) 
        {
            _httpClient= httpClient;
        }

        public async Task<GetDataForFilterEventRequest.Response> Handle(GetDataForFilterEventRequest request, CancellationToken cancellationToken)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<GetDataForFilterEventRequest.Response>(GetDataForFilterEventRequest.RouteTemplate);
            }
            catch (Exception ex)
            {
                return default!;
            }
        }

   
    }
}
