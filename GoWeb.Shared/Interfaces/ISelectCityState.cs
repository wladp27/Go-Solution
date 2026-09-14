using System;
using System.Collections.Generic;
using System.Text;

namespace GoWeb.Shared.Interfaces
{
    public interface ISelectCityState
    {
        int SelectedCityId { get; }

        event Action<int>? OnChange;

        Task Initialize();

        Task SetIdCity(int idCity);


    }
}
