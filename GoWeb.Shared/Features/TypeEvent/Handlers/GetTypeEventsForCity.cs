using GoWeb.Shared.Requests;
using MediatR;
using System.Net.Http.Json;

namespace GoWeb.Shared.Features.TypeEvent.Handlers
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
                var content = await httpResponse.Content.ReadFromJsonAsync<GetTypeEventsForCityRequest.Response>(cancellationToken: cancellationToken);
                if(content!=null)
                {
                    return content;
                }
                else
                {
                    throw new Exception("Ошибка при получении данных с сервера.");
                }
            }
            catch (HttpRequestException)
            {
                throw new Exception("Не удалось подключиться к серверу. Проверьте сеть.");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
