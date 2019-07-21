using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel;
using System.Net;
using System.Threading.Tasks;

namespace Verivox.API.Middleware
{
    /// <summary>
    /// Exception middleware for unhandled exceptions
    /// </summary>
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionMiddleware> logger;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="next">Next action in pipeline</param>
        /// <param name="loggerFactory">Logger for diagnostic</param>
        public ExceptionMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger<ExceptionMiddleware>();
            this.next = next;
        }

        /// <summary>
        /// Invoke method in pipeline
        /// </summary>
        /// <param name="httpContext">Current context</param>
        /// <returns>Void</returns>
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await next(httpContext);
            }
            catch (InvalidEnumArgumentException exception)
            {
                logger.LogError(16, exception, exception.Message);
                await HandleArgumentExceptionAsync(httpContext, exception);
            }
            catch (ArgumentOutOfRangeException exception)
            {
                logger.LogError(15, exception, exception.Message);
                await HandleArgumentExceptionAsync(httpContext, exception);
            }
            catch (ArgumentException exception)
            {
                logger.LogError(14, exception, exception.Message);
                await HandleArgumentExceptionAsync(httpContext, exception);
            }
            catch (Exception exception)
            {
                logger.LogError(13, exception, exception.Message);
                await HandleExceptionAsync(httpContext, exception);
            }
        }

        private static Task HandleArgumentExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            return context.Response.WriteAsync("Bad request, please check your input data.");
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            return context.Response.WriteAsync("Error on server side, please try again later or contact customers support center.");
        }
    }
}
