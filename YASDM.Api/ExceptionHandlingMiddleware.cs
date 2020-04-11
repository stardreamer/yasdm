using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using YASDM.Model.DTO;

namespace YASDM.Api
{


    public class ExeptionHandlingMiddleware
    {
        private static string FlattenException(Exception exception)
        {
            var stringBuilder = new StringBuilder();

            while (exception != null)
            {
                stringBuilder.AppendLine(exception.Message);
                stringBuilder.AppendLine(exception.StackTrace);

                exception = exception.InnerException;
            }

            return stringBuilder.ToString();
        }
        private readonly RequestDelegate next;
        private readonly bool _isDev;
        public ExeptionHandlingMiddleware(RequestDelegate next, bool isDev = false)
        {
            this.next = next;
            this._isDev = isDev;
        }

        public async Task Invoke(HttpContext context /* other dependencies */)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, _isDev);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex, bool isDev)
        {
            var code = HttpStatusCode.InternalServerError; // 500 if unexpected
            var message = "There was an unexpected error";

            if (ex is ApiNotFoundException)
            {
                code = HttpStatusCode.NotFound;
                message = ex.Message;
            }
            else if (ex is ApiUnauthorizedException || ex is ApiRefreshTokenExpiredException)
            {
                code = HttpStatusCode.Unauthorized;
                message = ex.Message;
            }
            else if (ex is ApiException)
            {
                code = HttpStatusCode.BadRequest;
                message = ex.Message;
            }

            var result = JsonConvert.SerializeObject(new ErrorDTO { Message = message, Trace = isDev ? FlattenException(ex) : "No stack trace" });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}