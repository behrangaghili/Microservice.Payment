using FluentValidation;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Postex.SharedKernel.Api;
using Postex.SharedKernel.Exceptions;
using System.Net;

namespace Postex.SharedKernel.Extensions
{
    internal static class HttpResponseExtensions
    {
        public static Task WriteValidationErrorsAsync(this HttpResponse response, ValidationException ex)
        {
            var errors = ex.Errors.Select(cur => cur.ErrorMessage)
                .Distinct()
                .ToList();

            var message =  string.Join(",", errors);
            return response.WriteResponseAsync(HttpStatusCode.BadRequest, new ApiResult(false, errors, message));
        }

        public static Task WriteEntityNotFoundAsync(this HttpResponse response, EntityNotFoundException ex)
        {
            return response.WriteResponseAsync(HttpStatusCode.NotFound, new ApiResult(false, ex.Message));
        }

        public static Task WriteApplicationErrorsAsync(this HttpResponse response, AppException ex)
        {
            return response.WriteResponseAsync(ex.HttpStatusCode, new ApiResult(false, ex.Message));
        }

        public static Task WriteBusinessErrorsAsync(this HttpResponse response, BusinessException ex)
        {
            return response.WriteResponseAsync(HttpStatusCode.BadRequest, new ApiResult(false, ex.Message));
        }

        public static Task WriteUnhandledExceptionsAsync(this HttpResponse response)
        {
            return response.WriteResponseAsync(HttpStatusCode.BadRequest, new ApiResult(false, "Internal server error"));
        }

        private static Task WriteResponseAsync(this HttpResponse response, HttpStatusCode statusCodes, ApiResult apiResult)
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver()
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                },
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore,
            };

            response.StatusCode = (int)statusCodes;
            response.ContentType = "application/json";

            return response.WriteAsync(JsonConvert.SerializeObject(apiResult, settings));
        }
    }
}
