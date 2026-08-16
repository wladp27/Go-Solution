using GoWeb.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Go.Mobile.Services
{
    internal class MauiTokenService : ITokenService
    {
        private const string TokenKey = "jwt_token";

        public async Task<string?> GetTokenAsync()
        {
            return await SecureStorage.Default.GetAsync(TokenKey);
        }

        public async Task SetTokenAsync(string token)
        {
            await SecureStorage.Default.SetAsync(TokenKey, token);
        }

        public async Task RemoveTokenAsync()
        {
            SecureStorage.Default.Remove(TokenKey);
        }
    }
}
