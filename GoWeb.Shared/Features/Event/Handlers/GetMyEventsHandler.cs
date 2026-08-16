using GoWeb.Shared.Requests;
using MediatR;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;

namespace GoWeb.Shared.Features.Event.Handlers
{
    public class GetMyEventsHandler : IRequestHandler<GetMyEventsRequest, GetMyEventsRequest.Response>
    {

        private readonly IHttpClientFactory httpClientFactory;
        public GetMyEventsHandler(IHttpClientFactory httpClientFactory) 
        {
            this.httpClientFactory = httpClientFactory;


        }
        public async Task<GetMyEventsRequest.Response> Handle(GetMyEventsRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var client = httpClientFactory.CreateClient("TokenAPIClient");
                var httpResponse = await client.GetAsync(GetMyEventsRequest.RouteTemplate);
                if (!httpResponse.IsSuccessStatusCode)
                {
                    if (httpResponse.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        var errorMessage = "Ошибка авторизации";
                        throw new Exception(errorMessage);
                    }     
                    throw new Exception();
                }
                var content = await httpResponse.Content.ReadFromJsonAsync<GetMyEventsRequest.Response>(cancellationToken: cancellationToken);
                if (content != null)
                    return content;
                throw new Exception();
            }
            catch (HttpRequestException)
            {
                throw new Exception("Не удалось подключиться к серверу. Проверьте сеть.");
            }
            catch (Exception ex)
            {
                if (ex.Message.StartsWith("Ошибка авторизации"))
                {
                    throw;
                }
                throw new Exception("Ошибка на стороне сервера");
            }
        }
    }
}
