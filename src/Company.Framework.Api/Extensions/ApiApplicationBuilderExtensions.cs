﻿using Company.Framework.Api.Handlers;
using Company.Framework.Api.Localization.Extensions;
using Company.Framework.Api.Middlewares;
using CorrelationId;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Company.Framework.Api.Extensions
{
    public static class ApiApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseApi(this IApplicationBuilder applicationBuilder)
        {
            return applicationBuilder
                .UseCorrelationId()
                .UseLocalization()
                .UseMiddleware<LogHandlerMiddleware>()
                .UseExceptionHandler(app => app.Run(app.ApplicationServices.GetRequiredService<ApiExceptionHandler>().Handle));
        }
    }
}
