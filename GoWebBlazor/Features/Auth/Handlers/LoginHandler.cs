using GoWeb.Shared.Requests;
using MediatR;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace GoWebBlazor.Features.Auth.Handlers
{
    public class LoginHandler : IRequestHandler<LoginRequest, LoginRequest.Response>
    {
        private readonly HttpClient httpClient;
        public LoginHandler(HttpClient httpClient) 
        {
            this.httpClient = httpClient;
        }
        public async Task<LoginRequest.Response> Handle(LoginRequest request, CancellationToken cancellationToken)
        {
            try
            {

                var httpResponse = await httpClient.PostAsJsonAsync(LoginRequest.RouteTemplate,request.userLoginDTO,cancellationToken);
                if (!httpResponse.IsSuccessStatusCode)
                {
                    var errorMessage = "";
                    if (httpResponse.StatusCode==HttpStatusCode.Unauthorized)
                        errorMessage = "Неверный логин или пароль";
                    throw new Exception(errorMessage);
                }
                var content = await httpResponse.Content.ReadFromJsonAsync<LoginRequest.Response>(cancellationToken: cancellationToken);
                if (content != null)
                {
                    return content;
                }
                throw new Exception();

            }
            catch (HttpRequestException)
            {
                throw new Exception("Не удалось подключиться к серверу. Проверьте сеть.");
            }

            catch (Exception ex)
            {
                if (ex.Message.StartsWith("Неверный логин или пароль"))
                {
                    throw; 
                }
                throw new Exception("Ошибка на стороне сервера");
            }

        }
    }
}
