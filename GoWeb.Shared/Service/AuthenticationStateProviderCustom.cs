using GoWeb.Shared.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace GoWeb.Shared.Service
{
    public class AuthenticationStateProviderCustom : AuthenticationStateProvider
    {
        private readonly ITokenService tokenService;
        public event Action? OnChange;
        public AuthenticationStateProviderCustom(ITokenService tokenService) 
        {
            this.tokenService = tokenService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {

            var token= await tokenService.GetTokenAsync();
            if (token != null)
            {
                var principal = ParseJwtToken(token);
                return new AuthenticationState(principal);
            }
            var anonymous = new ClaimsPrincipal(new ClaimsIdentity());
            return new AuthenticationState(anonymous);
        }

        public ClaimsPrincipal ParseJwtToken(string jwtToken)
        {
            var handler = new JwtSecurityTokenHandler();

            if (!handler.CanReadToken(jwtToken))
            {
     
                return new ClaimsPrincipal(new ClaimsIdentity());
            }

            var token = handler.ReadJwtToken(jwtToken);

            var identity = new ClaimsIdentity(token.Claims, "jwtAuthType"); //если не напишем jwtAuthType то авторизация не засчитается

            return new ClaimsPrincipal(identity);
        }

        public async Task LogoutAsync()
        {
            await tokenService.RemoveTokenAsync();
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public void NotifyUserAuthentication()
        {
            OnChange?.Invoke();
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
