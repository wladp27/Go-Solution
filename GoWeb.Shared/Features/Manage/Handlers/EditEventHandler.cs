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
    public class EditEventHandler : IRequestHandler<EditEventRequest, OperationResult<EditEventRequest.Response>>
    {
        private IHttpClientFactory httpClientFactory;
        public EditEventHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        public async Task<OperationResult<EditEventRequest.Response>> Handle(EditEventRequest request, CancellationToken cancellationToken)
        {
            var client = httpClientFactory.CreateClient("TokenAPIClient");
            return await client.SafePutAsJsonAsync(request,cancellationToken);
            
        }
    }
}
