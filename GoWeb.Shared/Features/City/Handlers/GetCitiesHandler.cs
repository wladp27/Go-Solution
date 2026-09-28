using GoWeb.Shared.Model;
using GoWeb.Shared.Requests;
using GoWeb.Shared.Service;
using MediatR;
using System.Net.Http.Json;

namespace GoWeb.Shared.Features.City.Handlers
{
    public class GetCitiesHandler : IRequestHandler<GetCitiesRequest, OperationResult<GetCitiesRequest.Response>>
    {
        private readonly HttpClient httpClient;
        public GetCitiesHandler(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
     
        public async Task<OperationResult<GetCitiesRequest.Response>> Handle(GetCitiesRequest request, CancellationToken cancellationToken)
        {
              return await httpClient.SafeGetAsJsonAsync<GetCitiesRequest.Response>(GetCitiesRequest.RouteTemplate, cancellationToken);
        }
    }
}
