using GoWeb.Shared.Model;
using GoWeb.Shared.Models;
using GoWeb.Shared.Requests;
using GoWeb.Shared.Service;
using MediatR;
using System.Net.Http.Json;

namespace GoWeb.Shared.Features.Event.Handlers
{
    public class GetPublishedEventsHandler : IRequestHandler<GetPublishedEventsRequest, OperationResult<GetPublishedEventsRequest.Response>>
    {
        private readonly HttpClient _httpClient;
        public GetPublishedEventsHandler(HttpClient httpClient) 
        {
            _httpClient = httpClient;
        }
        public async Task<OperationResult<GetPublishedEventsRequest.Response>> Handle(GetPublishedEventsRequest request, CancellationToken cancellationToken)
        {
            return await _httpClient.SafePostAsJsonAsync(request, cancellationToken);
        }
    }
}
