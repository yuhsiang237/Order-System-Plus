using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace OrderSystemPlus.Exceptions
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;
        public ExceptionMiddleware(RequestDelegate next)
        {
            
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (BusinessException ex)
            {
                await HandleBusinessExceptionAsync(context, ex);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleBusinessExceptionAsync(HttpContext context, BusinessException exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = exception.StatusCode;
            
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            var response = new
            {
                StatusCode = exception.StatusCode,
                Message = exception.Message,
            };

            await context.Response.WriteAsync(JsonConvert.SerializeObject(response, settings));
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            var response = new
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                Message = "Internal Server Error"
            };

            await context.Response.WriteAsync(JsonConvert.SerializeObject(response, settings));
        }
    }

    public static class ExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
