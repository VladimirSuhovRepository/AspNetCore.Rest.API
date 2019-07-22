using System;
using System.ComponentModel;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Verivox.API.Middleware
{
    /// <summary>
    /// Exception middleware for unhandled exceptions.
    /// </summary>
    public class ExceptionMiddleware
    {
        private const string BadRequestMessage = "Bad request, please check your input data.";
        private const string InternalErrorMessage = "Error on server side, please try again later or contact customers support center.";
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionMiddleware> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionMiddleware"/> class.
        /// </summary>
        /// <param name="next">Next action in pipeline.</param>
        /// <param name="loggerFactory">Logger for diagnostic.</param>
        public ExceptionMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            this.logger = loggerFactory.CreateLogger<ExceptionMiddleware>();
            this.next = next;
        }

        /// <summary>
        /// Invoke method in pipeline.
        /// </summary>
        /// <param name="httpContext">Current context.</param>
        /// <returns>Void.</returns>
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await this.next(httpContext).ConfigureAwait(false);
            }
            catch (InvalidEnumArgumentException exception)
            {
                this.logger.LogError(16, exception, exception.Message);
                await HandleArgumentExceptionAsync(httpContext).ConfigureAwait(false);
            }
            catch (ArgumentOutOfRangeException exception)
            {
                this.logger.LogError(15, exception, exception.Message);
                await HandleArgumentExceptionAsync(httpContext).ConfigureAwait(false);
            }
            catch (ArgumentException exception)
            {
                this.logger.LogError(14, exception, exception.Message);
                await HandleArgumentExceptionAsync(httpContext).ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                this.logger.LogError(13, exception, exception.Message);
                await HandleExceptionAsync(httpContext).ConfigureAwait(false);
            }
        }

        private static Task HandleArgumentExceptionAsync(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            return context.Response.WriteAsync(BadRequestMessage);
        }

        private static Task HandleExceptionAsync(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            return context.Response.WriteAsync(InternalErrorMessage);
        }
    }
}
