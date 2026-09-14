using Blazored.LocalStorage;
using GoWeb.Shared.Interfaces;
using GoWeb.Shared.Model;
using GoWeb.Shared.Service;
using Microsoft.AspNetCore.Components.Authorization;

namespace GoWebBlazor.Service.State
{
    public class SelectCityState :IDisposable, ISelectCityState
    {

        private readonly ILocalStorageService _localStorageService;

        public int SelectedCityId { get;private set;}

        private bool _isInitialized = false;
        public event Action<int>? OnChange;
        private readonly AuthenticationStateProviderCustom authenticationStateProviderCustom;


        public SelectCityState(ILocalStorageService localStorageService, AuthenticationStateProviderCustom authenticationStateProviderCustom) 
        {
            _localStorageService = localStorageService;
            this.authenticationStateProviderCustom = authenticationStateProviderCustom;
        }

        public async Task Initialize()
        {
            if (_isInitialized)
                return;
            authenticationStateProviderCustom.AuthenticationStateChanged += OnAuthenticationStateChanged;
            var authState = await authenticationStateProviderCustom.GetAuthenticationStateAsync();
            var cityFromClaims = GetIdCityInClaims(authState);

            if (cityFromClaims != 0)
            {
                await SetIdCity(cityFromClaims);
            }
            else
            {
                var savedCity = await _localStorageService.GetItemAsync<int>("idCity");
                SelectedCityId = savedCity;
                NotifyStateChanged();
            }
            _isInitialized = true;
        }

        public async Task SetIdCity(int idCity)
        {
            if (SelectedCityId == idCity)
                return; 
            SelectedCityId = idCity;
            await _localStorageService.SetItemAsync("idCity", idCity);
            NotifyStateChanged();
        }


        public  int GetIdCityInClaims(AuthenticationState authState)
        {
            var user = authState.User;
            if (user.Identity?.IsAuthenticated != true)
                return 0 ;
            var claimsIdCity = user.FindFirst(Claims.idCity);
            if (claimsIdCity != null && int.TryParse(claimsIdCity.Value, out int cityId))
                return cityId;
            return 0;
        }

        private async void OnAuthenticationStateChanged(Task<AuthenticationState> task)
        {
            try
            {
                var authState = await task;
                var idCityClaims =  GetIdCityInClaims(authState);
                await SetIdCity(idCityClaims);
            }
            catch
            {
                await SetIdCity(0);
            }
        }


        private void NotifyStateChanged() => OnChange?.Invoke(SelectedCityId);


        public void Dispose()
        {
            authenticationStateProviderCustom.AuthenticationStateChanged -= OnAuthenticationStateChanged;
        }


    }
}
