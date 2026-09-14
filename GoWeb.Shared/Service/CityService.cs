using GoWeb.Shared.Model;
using GoWeb.Shared.Models;
using GoWeb.Shared.Requests;
using MediatR;
using System.Threading.Tasks;


namespace GoWeb.Shared.Service
{
    public class CityService
    {
        private List<CityDTO> cityList = default!;

        private readonly IMediator mediator;
       
        public CityService(IMediator mediator)
        {
            this.mediator = mediator;
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
    }
}
