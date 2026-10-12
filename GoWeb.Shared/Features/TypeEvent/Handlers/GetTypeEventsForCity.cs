using GoWeb.Shared.Model;
using GoWeb.Shared.Requests;
using GoWeb.Shared.Service;
using MediatR;


namespace GoWeb.Shared.Features.TypeEvent.Handlers
{
    public class GetTypeEventsForCity : IRequestHandler<GetTypeEventsForCityRequest,OperationResult<GetTypeEventsForCityRequest.Response>>
    {
        private HttpClient httpClient;
        public GetTypeEventsForCity(HttpClient httpClient) 
        {
           this.httpClient = httpClient;
        }
        public async Task<OperationResult<GetTypeEventsForCityRequest.Response>> Handle(GetTypeEventsForCityRequest request, CancellationToken cancellationToken)
        {
         
                return await httpClient.SafeGetAsJsonAsync<GetTypeEventsForCityRequest.Response>(GetTypeEventsForCityRequest.RouteTemplate.Replace("{id}", request.idCity.ToString()),cancellationToken);     
        }
    }
}
