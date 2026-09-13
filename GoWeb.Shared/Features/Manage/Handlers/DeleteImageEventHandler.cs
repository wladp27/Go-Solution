using GoWeb.Shared.Requests;
using MediatR;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace GoWeb.Shared.Features.Manage.Handlers
{
    public class DeleteImageEventHandler : IRequestHandler<DeleteImageEventRequest, DeleteImageEventRequest.Response>
    {

        private IHttpClientFactory httpClientFactory;
        public DeleteImageEventHandler(IHttpClientFactory httpClientFactory) 
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<DeleteImageEventRequest.Response> Handle(DeleteImageEventRequest request, CancellationToken cancellationToken)
        {
            var client = httpClientFactory.CreateClient("TokenAPIClient");
            try
            {
                var httpResponse = await client.PutAsJsonAsync(DeleteImageEventRequest.RouteTemplate.Replace("{nameImage}", request.nameImage), cancellationToken);
                if (!httpResponse.IsSuccessStatusCode)
                {
                    var errorContent = await httpResponse.Content.ReadFromJsonAsync<DeleteImageEventRequest.Response>(cancellationToken);
                    return errorContent ?? DeleteImageEventRequest.Response.Failure("Произошла непредвиденная ошибка");
                }
                var content = await httpResponse.Content.ReadFromJsonAsync<DeleteImageEventRequest.Response>(cancellationToken);
                return content ?? DeleteImageEventRequest.Response.Failure("Получен пустой ответ от сервера");
            }
            catch (HttpRequestException)
            {
                return DeleteImageEventRequest.Response.Failure("Ошибка соединения с сервером");
            }
            catch (Exception)
            {

                return DeleteImageEventRequest.Response.Failure("Произошла непредвиденная ошибка");
            }
        }
    }
}
