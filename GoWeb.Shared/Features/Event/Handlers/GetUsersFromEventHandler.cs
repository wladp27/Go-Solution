using FluentResults;
using GoWeb.Shared.Requests;
using MediatR;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace GoWeb.Shared.Features.Event.Handlers
{
    public class GetUsersFromEventHandler : IRequestHandler<GetUsersFromEventRequest, GetUsersFromEventRequest.Response>
    {
        private readonly IHttpClientFactory httpClientFactory;
        public GetUsersFromEventHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        public async Task<GetUsersFromEventRequest.Response> Handle(GetUsersFromEventRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var client = httpClientFactory.CreateClient("TokenAPIClient");
                var url = GetUsersFromEventRequest.RouteTemplate.Replace("{id}", request.idEvent.ToString());
                var response = await client.GetAsync(url, cancellationToken);
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    var result = await response.Content.ReadFromJsonAsync<GetUsersFromEventRequest.Response>(cancellationToken);
                    return result ?? new GetUsersFromEventRequest.Response(false, "Ресурс не найден", new());
                }
                if (!response.IsSuccessStatusCode)
                {
                    return new GetUsersFromEventRequest.Response(false, "Ошибка сервера", new());
                }
                var content = await response.Content.ReadFromJsonAsync<GetUsersFromEventRequest.Response>(cancellationToken);
                return content ?? new GetUsersFromEventRequest.Response(true, null, new());
            }
            catch (HttpRequestException )
            {
                return new GetUsersFromEventRequest.Response(false, "Ошибка соединения с сервером", new());
            }
            catch (Exception )
            {

                return new GetUsersFromEventRequest.Response(false, "Произошла непредвиденная ошибка", new());
            }
        }
    }
}
