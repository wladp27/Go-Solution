using GoWeb.Shared.Requests;
using MediatR;
using Microsoft.VisualBasic;
using System.Net.Http.Json;


namespace GoWeb.Shared.Features.Event.Handlers
{
    public class JoinEventHandler : IRequestHandler<JoinEventRequest, JoinEventRequest.Response>
    {

        private readonly IHttpClientFactory httpClientFactory;
        public JoinEventHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        public async Task<JoinEventRequest.Response> Handle(JoinEventRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var client = httpClientFactory.CreateClient("TokenAPIClient");
                var response = await client.GetAsync(JoinEventRequest.RouteTemplate.Replace("{id}", request.id.ToString()));
                var statusJoin = await response.Content.ReadFromJsonAsync<JoinEventRequest.Response>(cancellationToken);
                return statusJoin ?? throw new Exception();
            }
            catch (HttpRequestException)
            {
                return new JoinEventRequest.Response("Error", "Ошибка соединения с сервером");
            }
            catch (Exception)
            {
                return new JoinEventRequest.Response("Error", "Произошла непредвиденная ошибка");
            }
        }
    }
}
