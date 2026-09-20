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
    public class CreateEventHandler : IRequestHandler<CreateEventRequest, CreateEventRequest.Response>
    {

        private IHttpClientFactory httpClientFactory;
        public CreateEventHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        public async Task<CreateEventRequest.Response> Handle(CreateEventRequest request, CancellationToken cancellationToken)
        {
            var client = httpClientFactory.CreateClient("TokenAPIClient");
            var baseResult = await client.SafePostAsJsonAsync(request, cancellationToken);
            return CreateEventRequest.Response.CastOperation(baseResult);
        }

    }
}
