using GoWeb.Shared.Model;
using GoWeb.Shared.Requests;
using GoWeb.Shared.Service;
using MediatR;

namespace GoWeb.Shared.Features.Event.Handlers
{
    public class GetEventWithUsersHandler : IRequestHandler<GetEventWithUsersRequest,OperationResult<GetEventWithUsersRequest.Response>>
    {
        private readonly IHttpClientFactory httpClientFactory;
        public GetEventWithUsersHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        public async Task<OperationResult<GetEventWithUsersRequest.Response>> Handle(GetEventWithUsersRequest request, CancellationToken cancellationToken)
        {
             var client = httpClientFactory.CreateClient("TokenAPIClient");
             return await  client.SafeGetAsJsonAsync<GetEventWithUsersRequest.Response>(GetEventWithUsersRequest.RouteTemplate.Replace("{id}", request.Id.ToString()), cancellationToken);
        }
    }
}
