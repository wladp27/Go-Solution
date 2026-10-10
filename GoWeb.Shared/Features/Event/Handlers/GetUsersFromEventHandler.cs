using FluentResults;
using GoWeb.Shared.Model;
using GoWeb.Shared.Requests;
using GoWeb.Shared.Service;
using MediatR;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace GoWeb.Shared.Features.Event.Handlers
{
    public class GetUsersFromEventHandler : IRequestHandler<GetUsersFromEventRequest,OperationResult<GetUsersFromEventRequest.Response>>
    {
        private readonly IHttpClientFactory httpClientFactory;
        public GetUsersFromEventHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        public async Task<OperationResult<GetUsersFromEventRequest.Response>> Handle(GetUsersFromEventRequest request, CancellationToken cancellationToken)
        {
            var client = httpClientFactory.CreateClient("TokenAPIClient");
            return await client.SafeGetAsJsonAsync<GetUsersFromEventRequest.Response>(GetUsersFromEventRequest.RouteTemplate.Replace("{id}", request.idEvent.ToString()), cancellationToken);
        }
    }
}
