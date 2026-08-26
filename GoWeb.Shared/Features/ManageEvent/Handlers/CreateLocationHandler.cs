using GoWeb.Shared.Requests;
using MediatR;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace GoWeb.Shared.Features.ManageEvent.Handlers
{
    public class CreateLocationHandler : IRequestHandler<CreateLocationRequest, CreateLocationRequest.Response>
    {
        private IHttpClientFactory httpClientFactory;
        public CreateLocationHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        public async Task<CreateLocationRequest.Response> Handle(CreateLocationRequest request, CancellationToken cancellationToken)
        {
            var client = httpClientFactory.CreateClient("TokenAPIClient");
            try
            {
                var httpResponse = await client.PostAsJsonAsync(CreateLocationRequest.RouteTemplate, request.Location, cancellationToken);
                if(!httpResponse.IsSuccessStatusCode)
                {
                    var errorContent = await httpResponse.Content.ReadFromJsonAsync<CreateLocationRequest.Response>(cancellationToken);
                    return errorContent ??  CreateLocationRequest.Response.Failure("Произошла непредвиденная ошибка");
                }
                var content = await httpResponse.Content.ReadFromJsonAsync<CreateLocationRequest.Response>(cancellationToken);
                return content ?? CreateLocationRequest.Response.Failure("Получен пустой ответ от сервера");
            }
            catch (HttpRequestException)
            {
                return  CreateLocationRequest.Response.Failure("Ошибка соединения с сервером");
            }
            catch (Exception)
            {

                return CreateLocationRequest.Response.Failure("Произошла непредвиденная ошибка");
            }

        }
    }
}
