using GoWeb.Shared.Requests;
using MediatR;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace GoWeb.Shared.Features.Manage.Handlers
{
    public class EditEventHandler : IRequestHandler<EditEventRequest, EditEventRequest.Response>
    {
        private IHttpClientFactory httpClientFactory;
        public EditEventHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        public async Task<EditEventRequest.Response> Handle(EditEventRequest request, CancellationToken cancellationToken)
        {
            var client = httpClientFactory.CreateClient("TokenAPIClient");
            try
            {
                var httpResponse = await client.PutAsJsonAsync(EditEventRequest.RouteTemplate.Replace("{id}", request.eventEdit.Id.ToString()), request.eventEdit, cancellationToken);
                if (!httpResponse.IsSuccessStatusCode)
                {
                    var errorContent = await httpResponse.Content.ReadFromJsonAsync<EditEventRequest.Response>(cancellationToken);
                    return errorContent ?? EditEventRequest.Response.Failure("Произошла непредвиденная ошибка");
                }
                var content = await httpResponse.Content.ReadFromJsonAsync<EditEventRequest.Response>(cancellationToken);
                return content ?? EditEventRequest.Response.Failure("Получен пустой ответ от сервера");
            }
            catch (HttpRequestException)
            {
                return EditEventRequest.Response.Failure("Ошибка соединения с сервером");
            }
            catch (Exception)
            {

                return EditEventRequest.Response.Failure("Произошла непредвиденная ошибка");
            }

        }
    }
}
