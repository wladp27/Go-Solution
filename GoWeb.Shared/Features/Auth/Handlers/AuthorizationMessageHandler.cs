using GoWeb.Shared.Interfaces;
using System.Net.Http.Headers;

namespace GoWeb.Shared.Features.Auth.Handlers
{
    public class AuthorizationMessageHandler:DelegatingHandler
    {

        private readonly ITokenService tokenService ;

        public AuthorizationMessageHandler(ITokenService tokenService)
        {
            this.tokenService = tokenService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage httpRequestMessage,CancellationToken cancellationToken)
        {
            var token = await tokenService.GetTokenAsync();
            if(!string.IsNullOrEmpty( token))
            {
                httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }    
            return await base.SendAsync(httpRequestMessage, cancellationToken);

        }

    }
}
