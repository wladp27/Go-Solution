using GoWeb.Shared.Interfaces;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoWeb.Shared.Service.State
{
    public class TokenState : ITokenService
    {
        private readonly IAppStorage _localStorageService;
        private const string TokenKey = "jwt_token";
        public TokenState(IAppStorage localStorageService)
        {
            _localStorageService = localStorageService;
        }

        public async Task<string?> GetTokenAsync()
        {
            return await _localStorageService.GetItemAsync<string>(TokenKey);
        }

        public async Task RemoveTokenAsync()
        {
            await _localStorageService.RemoveItemAsync(TokenKey);
        }

        public async Task SetTokenAsync(string token)
        {
            await _localStorageService.SetItemAsync(TokenKey, token);
        }

    }
}
