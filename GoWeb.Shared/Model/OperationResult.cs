using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoWeb.Shared.Model
{
    public class OperationResult
    {
        public OperationResult(bool isSuccess, string? errorMessage = null)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage ?? string.Empty;
        }
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }

        public static OperationResult Success() => new(true);
        public static OperationResult Failure(string error) => new(false, error);
    }

    public class OperationResult<T> : OperationResult
    {
        public OperationResult(bool isSuccess, T? data, string? errorMessage = null)
            : base(isSuccess, errorMessage)
        {
            Data = data;
        }
        public T? Data { get; set; }
        public static OperationResult<T> Success(T data) => new(true, data, string.Empty);
        public new static OperationResult<T> Failure(string error) => new(false, default, error);
    }
}
