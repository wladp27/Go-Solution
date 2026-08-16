using GoWeb.Shared.Interfaces;
using Microsoft.JSInterop;
namespace GoWebBlazor.Service
{
    public class TokenService : ITokenService
    {
        private readonly IJSRuntime jsRuntime;
        private const string TokenKey = "jwt_token";
        

        public TokenService(IJSRuntime jsRuntime)
        {
            this.jsRuntime = jsRuntime;
        }

        public async Task<string?> GetTokenAsync()
        {
           return await jsRuntime.InvokeAsync<string?>("localStorage.getItem",TokenKey);
        }

        public async Task RemoveTokenAsync()
        {
            await jsRuntime.InvokeVoidAsync("localStorage.removeItem", TokenKey);
        }

        public async Task SetTokenAsync(string token)
        {
            await jsRuntime.InvokeVoidAsync("localStorage.setItem", TokenKey, token);
        }
    }
}
