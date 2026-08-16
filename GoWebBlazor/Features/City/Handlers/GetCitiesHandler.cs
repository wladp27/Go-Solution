using GoWeb.Shared.Requests;
using MediatR;
using System.Net.Http.Json;

namespace GoWebBlazor.Features.City.Handlers
{
    public class GetCitiesHandler : IRequestHandler<GetCitiesRequest, GetCitiesRequest.Response>
    {
        private readonly HttpClient httpClient;
        public GetCitiesHandler(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
     
        public async Task<GetCitiesRequest.Response> Handle(GetCitiesRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var httpResponse = await httpClient.GetAsync(GetCitiesRequest.RouteTemplate);
                if (!httpResponse.IsSuccessStatusCode)
                {

                     var errorMessage = $"Ошибка сервера: {httpResponse.StatusCode} ({(int)httpResponse.StatusCode})";
                     throw new Exception(errorMessage);
                }
                return await httpResponse.Content.ReadFromJsonAsync<GetCitiesRequest.Response>(cancellationToken: cancellationToken);
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
