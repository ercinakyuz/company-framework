using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Formatting.Compact;

namespace Company.Framework.Logging.Extensions
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder WithSerilog(this IHostBuilder hostBuilder)
        {
            return hostBuilder
                .ConfigureLogging(loggingBuilder => loggingBuilder.ClearProviders())
                .UseSerilog((builderContext, loggerConfiguration) =>
                {
                    loggerConfiguration
                        .ReadFrom.Configuration(builderContext.Configuration)
                        .Enrich.FromLogContext()
                        .WriteTo.Async(configuration => configuration.Console(new RenderedCompactJsonFormatter()));
                });
        }

        public static IHostBuilder WithDefaultLogging(this IHostBuilder hostBuilder)
        {
            return hostBuilder.ConfigureLogging((context, loggingBuilder) =>
            {
                loggingBuilder
                    .ClearProviders()
                    .AddSimpleConsole(options => options.IncludeScopes = true);
            });
        }
    }
}
