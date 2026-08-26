using GoWeb.Shared.Requests;
using MediatR;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace GoWeb.Shared.Features.ManageEvent.Handlers
{
    public class GetFilteredLocationHandler : IRequestHandler<GetFilteredLocationsRequest, GetFilteredLocationsRequest.Response>
    {

        private IHttpClientFactory httpClientFactory;
        public GetFilteredLocationHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<GetFilteredLocationsRequest.Response> Handle(GetFilteredLocationsRequest request, CancellationToken cancellationToken)
        {
            var client = httpClientFactory.CreateClient("TokenAPIClient");
            try
            {
                var httpResponse = await client.GetAsync(GetFilteredLocationsRequest.RouteTemplate
                                                                    .Replace("{idCity}", request.IdCity.ToString())
                                                                    .Replace("{address}", Uri.EscapeDataString(request.Address)), cancellationToken);
                if (!httpResponse.IsSuccessStatusCode)
                {
                    var errorContent = await httpResponse.Content.ReadFromJsonAsync<GetFilteredLocationsRequest.Response>(cancellationToken);
                    return errorContent ?? GetFilteredLocationsRequest.Response.Failure($"Ошибка сервера: {httpResponse.StatusCode}");
                }
                var content = await httpResponse.Content.ReadFromJsonAsync<GetFilteredLocationsRequest.Response>(cancellationToken);
                return content ?? GetFilteredLocationsRequest.Response.Failure("Получен пустой ответ от сервера");
            }
            catch (HttpRequestException)
            {
                return GetFilteredLocationsRequest.Response.Failure("Ошибка соединения с сервером");
            }
            catch (Exception)
            {

                return GetFilteredLocationsRequest.Response.Failure("Произошла непредвиденная ошибка");
            }

        }
    }
}
