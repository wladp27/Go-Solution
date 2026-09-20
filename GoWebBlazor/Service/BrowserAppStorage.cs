using Blazored.LocalStorage;
using GoWeb.Shared.Interfaces;

namespace GoWebBlazor.Service
{
    public class BrowserAppStorage : IAppStorage
    {
        private readonly ILocalStorageService _localStorage;

        public BrowserAppStorage(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public async Task SetItemAsync<T>(string key, T item) => await _localStorage.SetItemAsync(key, item);

        public async Task<T?> GetItemAsync<T>(string key) => await _localStorage.GetItemAsync<T>(key);

        public async Task RemoveItemAsync(string key) => await _localStorage.RemoveItemAsync(key);
    }
}
