using GoWeb.Shared.Requests;
using MediatR;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace GoWeb.Shared.Features.Event.Handlers
{
    public class GetEventWithUsersHandler : IRequestHandler<GetEventWithUsersRequest, GetEventWithUsersRequest.Response>
    {
        private readonly IHttpClientFactory httpClientFactory;
        public GetEventWithUsersHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        public async Task<GetEventWithUsersRequest.Response> Handle(GetEventWithUsersRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var client = httpClientFactory.CreateClient("TokenAPIClient");
                var response = await client.GetAsync(GetEventWithUsersRequest.RouteTemplate.Replace("{id}", request.Id.ToString()), cancellationToken);
                var content = await response.Content.ReadFromJsonAsync<GetEventWithUsersRequest.Response>(cancellationToken);
                if (response.StatusCode== System.Net.HttpStatusCode.NotFound)
                {
                    return content ?? new GetEventWithUsersRequest.Response(false, "Event not found", new());
                }
                if (!response.IsSuccessStatusCode)
                {
                    return new GetEventWithUsersRequest.Response(false, "Ошибка сервера", new());
                }
                return content ?? new GetEventWithUsersRequest.Response(true, null, new());
            }
            catch (HttpRequestException)
            {
                return new GetEventWithUsersRequest.Response(false, "Ошибка соединения с сервером", new());
            }
            catch (Exception)
            {

                return new GetEventWithUsersRequest.Response(false, "Произошла непредвиденная ошибка", new());
            }

        }
    }
}
