using GoWeb.Shared.Requests;
using MediatR;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace GoWeb.Shared.Features.Manage.Handlers
{
    public class CreateEventHandler : IRequestHandler<CreateEventRequest, CreateEventRequest.Response>
    {

        private IHttpClientFactory httpClientFactory;
        public CreateEventHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        public async Task<CreateEventRequest.Response> Handle(CreateEventRequest request, CancellationToken cancellationToken)
        {
            var client = httpClientFactory.CreateClient("TokenAPIClient");
            try
            {
                var httpResponse = await client.PostAsJsonAsync(CreateEventRequest.RouteTemplate, request.eventCreate, cancellationToken);
                if (!httpResponse.IsSuccessStatusCode)
                {
                    var errorContent = await httpResponse.Content.ReadFromJsonAsync<CreateEventRequest.Response>(cancellationToken);
                    return errorContent ?? CreateEventRequest.Response.Failure("Произошла непредвиденная ошибка");
                }
                var content = await httpResponse.Content.ReadFromJsonAsync<CreateEventRequest.Response>(cancellationToken);
                return content ?? CreateEventRequest.Response.Failure("Получен пустой ответ от сервера");
            }
            catch (HttpRequestException)
            {
                return CreateEventRequest.Response.Failure("Ошибка соединения с сервером");
            }
            catch (Exception)
            {

                return CreateEventRequest.Response.Failure("Произошла непредвиденная ошибка");
            }

        }
      
    }
}
