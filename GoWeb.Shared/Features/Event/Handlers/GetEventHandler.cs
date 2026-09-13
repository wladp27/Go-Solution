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
                var response = await _httpClient.GetAsync(GetEventRequest.RouteTemplate.Replace("{id}", request.idEvent.ToString()), cancellationToken);
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadFromJsonAsync<GetEventRequest.Response>(cancellationToken);
                    return errorContent ?? GetEventRequest.Response.Failure("Произошла непредвиденная ошибка");
                }
                var content = await response.Content.ReadFromJsonAsync<GetEventRequest.Response>(cancellationToken);
                return content ?? GetEventRequest.Response.Failure("Получен пустой ответ от сервера");

            }
            catch (HttpRequestException)
            {
                return  GetEventRequest.Response.Failure("Ошибка соединения с сервером");
            }
            catch (Exception)
            {

                return  GetEventRequest.Response.Failure( "Произошла непредвиденная ошибка");
            }
        }
    }
}
