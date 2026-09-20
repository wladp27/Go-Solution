using GoWeb.Shared.Interfaces;
using GoWeb.Shared.Model;
using GoWeb.Shared.Requests;
using System.Net;
using System.Net.Http.Json;

namespace GoWeb.Shared.Service
{
    public static class HttpClientExtensions
    {
       public static async Task<OperationResult<TData>> SafePostAsJsonAsync<TModel, TData>(this HttpClient client, IRequestCastom<TModel, TData, OperationResult<TData>> request, CancellationToken cancellationToken)
       {
            try
            {
                var httpResponse = await client.PostAsJsonAsync(request.RouteTemplate, request.Model, cancellationToken);
                if (httpResponse.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return OperationResult<TData>.Failure("Ошибка авторизации");
                }
                if (httpResponse.StatusCode == HttpStatusCode.Forbidden)
                {
                    return OperationResult<TData>.Failure("У вас нет прав для выполнения этого действия.");
                }
                if (!httpResponse.IsSuccessStatusCode)
                {
                    var errorContent = await httpResponse.Content.ReadFromJsonAsync<OperationResult<TData>>(cancellationToken);
                    return errorContent ??  OperationResult<TData>.Failure("Произошла непредвиденная ошибка");
                }
                var content = await httpResponse.Content.ReadFromJsonAsync<OperationResult<TData>>(cancellationToken);
                return content ?? OperationResult<TData>.Failure("Получен пустой ответ от сервера");
            }
            catch (HttpRequestException)
            {
                return OperationResult<TData>.Failure("Ошибка соединения с сервером");
            }
            catch (TaskCanceledException)
            {
                return OperationResult<TData>.Failure("Время ожидания запроса истекло или он был отменен");
            }
            catch (Exception)
            {

                return OperationResult<TData>.Failure("Произошла непредвиденная ошибка");
            }
        }
    }
}
