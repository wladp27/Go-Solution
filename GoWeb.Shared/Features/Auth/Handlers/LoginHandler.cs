using GoWeb.Shared.Model;
using GoWeb.Shared.Requests;
using GoWeb.Shared.Service;
using MediatR;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace GoWeb.Shared.Features.Auth.Handlers
{
    public class LoginHandler : IRequestHandler<LoginRequest, OperationResult<LoginRequest.Response>>
    {
        private readonly HttpClient httpClient;
        public LoginHandler(HttpClient httpClient) 
        {
            this.httpClient = httpClient;
        }
        public async Task<OperationResult<LoginRequest.Response>> Handle(LoginRequest request, CancellationToken cancellationToken)
        {

           return await httpClient.SafePostAsJsonAsync(request,cancellationToken);
        }
    }
}
