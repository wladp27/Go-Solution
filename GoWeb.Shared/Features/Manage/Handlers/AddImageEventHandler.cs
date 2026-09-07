using GoWeb.Shared.Requests;
using MediatR;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace GoWeb.Shared.Features.Manage.Handlers
{
    public class AddImageEventHandler : IRequestHandler<AddImageEventRequest, AddImageEventRequest.Response>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public AddImageEventHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }   

        public async Task<AddImageEventRequest.Response> Handle(AddImageEventRequest request, CancellationToken cancellationToken)
        {
            var client = httpClientFactory.CreateClient("TokenAPIClient");
            try
            {
                using var contentImage = new MultipartFormDataContent();
                using var fileStream = request.Image.OpenReadStream(maxAllowedSize: 1024 * 1024 * 10);
                contentImage.Add(new StreamContent(fileStream), "image", request.Image.Name);
                var httpResponse = await client.PostAsync(AddImageEventRequest.RouteTemplate, contentImage, cancellationToken);

                if (!httpResponse.IsSuccessStatusCode)
                {
                    var errorContent = await httpResponse.Content.ReadFromJsonAsync<AddImageEventRequest.Response>(cancellationToken);
                    return errorContent ?? AddImageEventRequest.Response.Failure("Произошла непредвиденная ошибка");
                }
                var content = await httpResponse.Content.ReadFromJsonAsync<AddImageEventRequest.Response>(cancellationToken);
                return content ?? AddImageEventRequest.Response.Failure("Получен пустой ответ от сервера");
            }
            catch (HttpRequestException)
            {
                return AddImageEventRequest.Response.Failure("Ошибка соединения с сервером");
            }
            catch (Exception)
            {

                return AddImageEventRequest.Response.Failure("Произошла непредвиденная ошибка");
            }
        }
    }
}
