using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoWeb.Shared.Model
{
    public class OperationResult<T>
    {
        public OperationResult(bool result,T value,string message)
        {
            IsSuccess = result;
            Data = value;
            ErrorMessage = message;
        }

        public bool IsSuccess { get; set; }
        public T? Data { get; set; }
        public string? ErrorMessage { get; set; }
        public static OperationResult<T> Success(T data) => new OperationResult<T>(true, data, string.Empty);
        public static OperationResult<T> Failure(string error) => new OperationResult<T>(false, default!, error);
    }
}
