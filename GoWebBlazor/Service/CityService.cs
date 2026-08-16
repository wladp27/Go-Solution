using GoWeb.Shared.Model;
using GoWeb.Shared.Models;
using GoWeb.Shared.Requests;
using MediatR;
using System.Threading.Tasks;


namespace GoWebBlazor.Service
{
    public class CityService
    {
        private int _idSelectedCity;
        public int SelectedCity => _idSelectedCity;
 
        public event Action? OnChange;

        private List<CityDTO> cityList = default!;

        private readonly IMediator mediator;
        private readonly AuthenticationStateProviderCustom authenticationStateProviderCustom;
        public CityService(IMediator mediator, AuthenticationStateProviderCustom authenticationStateProviderCustom)
        {
            this.authenticationStateProviderCustom = authenticationStateProviderCustom;
            this.mediator = mediator;
        }

    
        public void SetIdCity(int idCity)
        {
            if (_idSelectedCity != idCity)
            {
                _idSelectedCity = idCity;
                NotifyStateChanged();
            }

        }

        public async Task<int> GetIdCityInClaims()
        {
            var authState = await authenticationStateProviderCustom.GetAuthenticationStateAsync();
            var user=authState.User;
            if (user.Identity?.IsAuthenticated != true)
            {
                _idSelectedCity = 0; 
                return _idSelectedCity;
            }
            var claimsIdCity = user.FindFirst(Claims.idCity);
            if (claimsIdCity != null && int.TryParse(claimsIdCity.Value, out int cityId))
            {
                _idSelectedCity = cityId;
                NotifyStateChanged();
            }
            else
            {
                _idSelectedCity = 0;
            }
            return _idSelectedCity;
        }

        public async Task<OperationResult<List<CityDTO>>> GetCities()
        {
            if(cityList == null)
            {
                try
                {
                    var response = await mediator.Send(new GetCitiesRequest());
                    cityList = response.Cities ?? new();
                    return OperationResult<List<CityDTO>>.Success(cityList);
                }
                catch(Exception ex) 
                {
                    return OperationResult<List<CityDTO>>.Failure(ex.Message);
                }
            }
            return OperationResult<List<CityDTO>>.Success(cityList);
        }
        private void NotifyStateChanged() => OnChange?.Invoke();

    

    }
}
