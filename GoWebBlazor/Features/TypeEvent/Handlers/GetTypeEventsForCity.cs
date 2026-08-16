using GoWeb.Shared.Requests;
using MediatR;
using System.Net.Http.Json;

namespace GoWebBlazor.Features.TypeEvent.Handlers
{
    public class GetTypeEventsForCity : IRequestHandler<GetTypeEventsForCityRequest, GetTypeEventsForCityRequest.Response>
    {
        private HttpClient httpClient;
        public GetTypeEventsForCity(HttpClient httpClient) 
        {
           this.httpClient = httpClient;
        }
        public async Task<GetTypeEventsForCityRequest.Response> Handle(GetTypeEventsForCityRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var httpResponse  = await httpClient.GetAsync(GetTypeEventsForCityRequest.RouteTemplate.Replace("{idCity}", request.idCity.ToString()));
                if (!httpResponse.IsSuccessStatusCode)
                {
                    var errorMessage = $"Ошибка сервера: {httpResponse.StatusCode} ({(int)httpResponse.StatusCode})";
                    throw new Exception(errorMessage);
                }
                return await httpResponse.Content.ReadFromJsonAsync<GetTypeEventsForCityRequest.Response>(cancellationToken: cancellationToken);
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Не удалось подключиться к серверу. Проверьте сеть.");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
