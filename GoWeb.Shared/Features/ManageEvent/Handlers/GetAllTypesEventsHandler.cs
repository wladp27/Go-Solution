using GoWeb.Shared.Requests;
using MediatR;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace GoWeb.Shared.Features.ManageEvent.Handlers
{
    public class GetAllTypesEventsHandler : IRequestHandler<GetAllTypesEventsRequest, GetAllTypesEventsRequest.Response>
    {
        private IHttpClientFactory httpClientFactory;
        public GetAllTypesEventsHandler(IHttpClientFactory httpClientFactory) 
        { 
            this.httpClientFactory = httpClientFactory;
        }
        public async Task<GetAllTypesEventsRequest.Response> Handle(GetAllTypesEventsRequest request, CancellationToken cancellationToken)
        {
            var client = httpClientFactory.CreateClient("TokenAPIClient");
            try
            {
                var httpResponse = await client.GetAsync(GetAllTypesEventsRequest.RouteTemplate, cancellationToken);
                var content = await httpResponse.Content.ReadFromJsonAsync<GetAllTypesEventsRequest.Response>(cancellationToken);
                if (httpResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return content ?? new GetAllTypesEventsRequest.Response(false, "Event not found", new());
                }
                if (!httpResponse.IsSuccessStatusCode)
                {
                    return new GetAllTypesEventsRequest.Response(false, "Ошибка сервера", new());
                }
                return content ?? new GetAllTypesEventsRequest.Response(true, null, new());
            }
            catch (HttpRequestException)
            {
                return new GetAllTypesEventsRequest.Response(false, "Ошибка соединения с сервером", new());
            }
            catch (Exception)
            {

                return new GetAllTypesEventsRequest.Response(false, "Произошла непредвиденная ошибка", new());
            }

        }
    }
}
