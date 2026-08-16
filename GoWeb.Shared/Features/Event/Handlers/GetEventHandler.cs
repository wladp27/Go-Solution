using GoWeb.Shared.Models;
using GoWeb.Shared.Requests;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net.Http.Json;

namespace GoWeb.Shared.Features.Event.Handlers
{
    public class GetEventHandler : IRequestHandler<GetEventRequest, GetEventRequest.Response>
    {
        private readonly HttpClient _httpClient;
        public GetEventHandler(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<GetEventRequest.Response?> Handle(GetEventRequest request, CancellationToken cancellationToken)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<GetEventRequest.Response>(GetEventRequest.RouteTemplate);
            }
            catch (HttpRequestException)
            {
                return default!;
            }
        }
    }
}
