using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SignalToAnswer.Exceptions;
using SignalToAnswer.Integrations.TriviaApi.Exceptions;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace SignalToAnswer.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogError(ex, ex.Message);

                var statusCode = (int)HttpStatusCode.BadRequest;

                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = statusCode;

                var response = new ApiException(statusCode, ex.Message);

                await WriteAsync(response, httpContext);
            }
            catch(TriviaApiServerException ex)
            {
                _logger.LogError(ex, ex.Message);

                var statusCode = (int)HttpStatusCode.InternalServerError;

                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = statusCode;

                var response = new ApiException(statusCode, ex.Message);

                await WriteAsync(response, httpContext);
            }
            catch (SignalToAnswerException ex)
            {
                _logger.LogError(ex, ex.Message);

                var statusCode = (int)HttpStatusCode.InternalServerError;

                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = statusCode;

                var response = new ApiException(statusCode, ex.Message);

                await WriteAsync(response, httpContext);
            }
            catch (ValidationException ex)
            {
                _logger.LogError(ex, ex.Message);

                var statusCode = (int)HttpStatusCode.BadRequest;

                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = statusCode;

                var response = new ApiException(statusCode, ex.Message, ex.ValidationErrors);

                await WriteAsync(response, httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                var statusCode = (int)HttpStatusCode.BadRequest;

                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = statusCode;

                var response = new ApiException(statusCode, ex.Message);

                await WriteAsync(response, httpContext);
            }
        }

        private async Task WriteAsync(ApiException ex, HttpContext httpContext)
        {
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            var json = JsonSerializer.Serialize(ex, options);

            await httpContext.Response.WriteAsync(json);
        }

        private class ApiException
        {
            public int StatusCode { get; set; }

            public string Message { get; set; }

            public object Details { get; set; }

            public ApiException(int statusCode, string message, object details = null)
            {
                StatusCode = statusCode;
                Message = message;
                Details = details;
            }
        }
    }
}
