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
    public class AddImageEventHandler : IRequestHandler<AddImageEventRequest,OperationResult<AddImageEventRequest.Response>>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public AddImageEventHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }   

        public async Task<OperationResult<AddImageEventRequest.Response>> Handle(AddImageEventRequest request, CancellationToken cancellationToken)
        {
            var client = httpClientFactory.CreateClient("TokenAPIClient");
            try
            {
                using var contentImage = new MultipartFormDataContent();
                using var fileStream = request.Image.OpenReadStream(maxAllowedSize: 1024 * 1024 * 10);
                contentImage.Add(new StreamContent(fileStream), "image", request.Image.Name);
                return await client.SafePostAsJsonAsync(request, contentImage, cancellationToken);
            }
            catch (Exception)
            {

                return OperationResult<AddImageEventRequest.Response>.Failure("Произошла непредвиденная ошибка");
            }
        }
    }
}
