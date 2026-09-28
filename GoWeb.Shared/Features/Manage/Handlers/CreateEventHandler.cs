using GoWeb.Shared.Model;
using GoWeb.Shared.Requests;
using GoWeb.Shared.Service;
using MediatR;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace GoWeb.Shared.Features.Manage.Handlers
{
    public class CreateEventHandler : IRequestHandler<CreateEventRequest, OperationResult<CreateEventRequest.Response>>
    {

        private IHttpClientFactory httpClientFactory;
        public CreateEventHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        public async Task<OperationResult<CreateEventRequest.Response>> Handle(CreateEventRequest request, CancellationToken cancellationToken)
        {
            var client = httpClientFactory.CreateClient("TokenAPIClient");
            return await client.SafePostAsJsonAsync(request, cancellationToken);
        }

    }
}
