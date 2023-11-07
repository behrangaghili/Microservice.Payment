using Microsoft.AspNetCore.Mvc;

namespace Postex.SharedKernel.Api
{
    public class ApiResult
    {
        public IList<string> Errors { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

        public ApiResult(bool isSuccess, string message = null)
        {
            IsSuccess = isSuccess;
            Message = message;
        }

        public ApiResult(bool isSuccess,IList<string> errors, string message = null)
        {
            IsSuccess = isSuccess;
            Message = message;
            Errors= errors;
        }

        #region Implicit Operators
        public static implicit operator ApiResult(OkResult result)
        {
            return new ApiResult(true);
        }

        public static implicit operator ApiResult(BadRequestResult result)
        {
            return new ApiResult(false);
        }

        public static implicit operator ApiResult(BadRequestObjectResult result)
        {
            var message = result.Value?.ToString();
            if (result.Value is SerializableError errors)
            {
                var errorMessages = errors.SelectMany(p => (string[])p.Value).Distinct();
                message = string.Join(" | ", errorMessages);
            }
            return new ApiResult(false, message);
        }

        public static implicit operator ApiResult(ContentResult result)
        {
            return new ApiResult(true, result.Content);
        }

        public static implicit operator ApiResult(NotFoundResult result)
        {
            return new ApiResult(false);
        }

        public static implicit operator ApiResult(UnauthorizedResult result)
        {
            return new ApiResult(false);
        }
        #endregion
    }

    public class ApiResult<TData> : ApiResult
    {
        public TData Data { get; set; }

        public ApiResult(bool isSuccess, TData data, string message = null)
            : base(isSuccess, message)
        {
            Data = data;
        }

        public ApiResult(bool isSuccess, string message = null)
            : base(isSuccess, message)
        {
        }

        #region Implicit Operators
        public static implicit operator ApiResult<TData>(TData data)
        {
            return new ApiResult<TData>(true, data);
        }

        public static implicit operator ApiResult<TData>(OkObjectResult result)
        {
            return new ApiResult<TData>(true, (TData)result.Value);
        }

        public static implicit operator ApiResult<TData>(BadRequestObjectResult result)
        {
            var message = result.Value?.ToString();
            if (result.Value is SerializableError errors)
            {
                var errorMessages = errors.SelectMany(p => (string[])p.Value).Distinct();
                message = string.Join(" | ", errorMessages);
            }
            return new ApiResult<TData>(false, message);
        }

        public static implicit operator ApiResult<TData>(ContentResult result)
        {
            return new ApiResult<TData>(true, result.Content);
        }

        public static implicit operator ApiResult<TData>(NotFoundObjectResult result)
        {
            return new ApiResult<TData>(false, (TData)result.Value);
        }
        #endregion
    }
}
