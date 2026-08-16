using GoWeb.Shared.Models;
using GoWeb.Shared.Requests;
using MediatR;
using System.Net.Http.Json;

namespace GoWebBlazor.Features.EventCatalogPage.Handlers
{
    public class GetPublishedEventsHandler : IRequestHandler<GetPublishedEventsRequest, GetPublishedEventsRequest.Response>
    {
        private readonly HttpClient _httpClient;
        public GetPublishedEventsHandler(HttpClient httpClient) 
        {
            _httpClient = httpClient;
        }
        public async Task<GetPublishedEventsRequest.Response> Handle(GetPublishedEventsRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(GetPublishedEventsRequest.RouteTemplate, request.filter);
                var responseListEvents = await response.Content.ReadFromJsonAsync<GetPublishedEventsRequest.Response>();
                return responseListEvents;
            }

            catch (Exception ex) 
            {
               return default!;
            }
        }
    }
}
