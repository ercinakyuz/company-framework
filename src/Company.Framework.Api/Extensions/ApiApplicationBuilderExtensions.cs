using Company.Framework.Api.Localization.Extensions;
using Company.Framework.Api.Middlewares;
using CorrelationId;
using Microsoft.AspNetCore.Builder;

namespace Company.Framework.Api.Extensions
{
    public static class ApiApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseApi(this WebApplication webApplication)
        {
            webApplication.MapControllers();
            return webApplication
                .UseSwagger()
                .UseSwaggerUI()
                .UseCorrelationId()
                .UseLocalization()
                .UseMiddleware<LogHandlerMiddleware>()
                .UseExceptionHandler(options=>{});


        }
    }
}
