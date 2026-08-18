using GoWeb.Shared.Requests;
using MediatR;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace GoWeb.Shared.Features.Event.Handlers
{
    public class LeaveEventHandler : IRequestHandler<LeaveEventRequest, LeaveEventRequest.Response>
    {

        private readonly IHttpClientFactory httpClientFactory;
        public LeaveEventHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        public async Task<LeaveEventRequest.Response> Handle(LeaveEventRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var client = httpClientFactory.CreateClient("TokenAPIClient");
                var response = await client.GetAsync(LeaveEventRequest.RouteTemplate.Replace("{id}", request.id.ToString()));
                var statusLeave = await response.Content.ReadFromJsonAsync<LeaveEventRequest.Response>(cancellationToken);
                return statusLeave ?? throw new Exception();
            }
            catch (HttpRequestException)
            {
                return new LeaveEventRequest.Response("Error", "Ошибка соединения с сервером");
            }
            catch (Exception)
            {
                return new LeaveEventRequest.Response("Error", "Произошла непредвиденная ошибка");
            }
        }
    }
}
