using GoWeb.Shared.Interfaces;
using GoWeb.Shared.Model;
using GoWeb.Shared.Requests;
using System.Net;
using System.Net.Http.Json;

namespace GoWeb.Shared.Service
{
    public static class HttpClientExtensions
    {
        public static async Task<OperationResult<TModelResponse>> SafePostAsJsonAsync<TRequestModel, TModelResponse>(
                                                                     this HttpClient client,
                                                                     IRequestPost<TRequestModel, TModelResponse> request,
                                                                     CancellationToken cancellationToken)
        {
            try
            {
                var httpResponse = await client.PostAsJsonAsync(request.RouteTemplate, request.Model, cancellationToken);

                if (httpResponse.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return OperationResult<TModelResponse>.Failure("Ошибка авторизации");
                }
                if (httpResponse.StatusCode == HttpStatusCode.Forbidden)
                {
                    return OperationResult<TModelResponse>.Failure("У вас нет прав для выполнения этого действия.");
                }
                if (!httpResponse.IsSuccessStatusCode)
                {
                    try
                    {
                        var errorContent = await httpResponse.Content.ReadFromJsonAsync<ValidationProblemDetails>(cancellationToken: cancellationToken);
                        var errorMessage = errorContent?.Detail ?? errorContent?.Title ?? "Произошла непредвиденная ошибка";
                        return OperationResult<TModelResponse>.Failure(errorMessage);
                    }
                    catch
                    {
                        return OperationResult<TModelResponse>.Failure($"Ошибка сервера: {(int)httpResponse.StatusCode}");
                    }
                }

                var content = await httpResponse.Content.ReadFromJsonAsync<TModelResponse>(cancellationToken: cancellationToken);

                if (content is null)
                {
                    return OperationResult<TModelResponse>.Failure("Получен пустой ответ от сервера");
                }

                return OperationResult<TModelResponse>.Success(content); 

            }
            catch (HttpRequestException)
            {
                return OperationResult<TModelResponse>.Failure("Ошибка соединения с сервером");
            }
            catch (TaskCanceledException)
            {
                return OperationResult<TModelResponse>.Failure("Время ожидания запроса истекло или он был отменен");
            }
            catch (Exception)
            {
                return OperationResult<TModelResponse>.Failure("Произошла непредвиденная ошибка на клиенте");
            }
        }
    }
}
