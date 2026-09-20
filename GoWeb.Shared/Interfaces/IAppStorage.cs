using System;
using System.Collections.Generic;
using System.Text;

namespace GoWeb.Shared.Interfaces
{
    public interface IAppStorage
    {
        Task SetItemAsync<T>(string key, T item);
        Task<T?> GetItemAsync<T>(string key);
        Task RemoveItemAsync(string key);
    }
}
