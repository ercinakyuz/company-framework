using Microsoft.Extensions.DependencyInjection;
using Company.Framework.Correlation.Extensions;
using Company.Framework.Core.Tenancy.Extensions;
using Company.Framework.Domain.Model.Aggregate.Event.Dispatcher;

namespace Company.Framework.Infrastructure.Application.Context.Extensions
{
    public static class ApplicationContextServiceCollectionExtensions
    {
        public static ApplicationContextServiceBuilder AddNonApiApplicationContext(this IServiceCollection services)
        {
            services.AddSingleton<IApplicationContextBuilder, NonApiApplicationContextBuilder>();
            return new ApplicationContextServiceBuilder(services);
        }
    }

    public class ApplicationContextServiceBuilder
    {
        private IServiceCollection _services;

        public ApplicationContextServiceBuilder(IServiceCollection services)
        {
            _services = services;
        }

        public ApplicationContextServiceBuilder WithCorrelation()
        {
            _services.AddCorrelation();
            return this;
        }

        public ApplicationContextServiceBuilder WithTenancy()
        {
            _services.AddTenancy();
            return this;
        }

        public IServiceCollection Build()
        {
            return _services;
        }
    }

}
