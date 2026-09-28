using GoWeb.Shared.Model;
using GoWeb.Shared.Models;
using GoWeb.Shared.Requests;
using GoWeb.Shared.Service;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net.Http.Json;

namespace GoWeb.Shared.Features.Event.Handlers
{
    public class GetEventHandler : IRequestHandler<GetEventRequest, OperationResult<GetEventRequest.Response>>
    {
        private readonly HttpClient _httpClient;

        public GetEventHandler(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<OperationResult<GetEventRequest.Response>> Handle(GetEventRequest request, CancellationToken cancellationToken)
        {
          return  await _httpClient.SafeGetAsJsonAsync<GetEventRequest.Response>(GetEventRequest.RouteTemplate.Replace("{id}", request.idEvent.ToString()), cancellationToken);
        }
    }
}
