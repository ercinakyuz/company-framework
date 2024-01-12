using Company.Framework.Core.Tenancy.Models;
using Company.Framework.Infrastructure.Application.Context.Builder.Args;
using CorrelationId.Abstractions;

namespace Company.Framework.Domain.Model.Aggregate.Event.Dispatcher
{
    public class NonApiApplicationContextBuilder: IApplicationContextBuilder
    {
        private readonly ICorrelationContextFactory _correlationContextFactory;


        private readonly ITenantBuilder _tenantBuilder;

        public NonApiApplicationContextBuilder(ICorrelationContextFactory correlationContextFactory, ITenantBuilder tenantBuilder)
        {
            _correlationContextFactory = correlationContextFactory;
            _tenantBuilder = tenantBuilder;
        }


        public void Build(ApplicationContextBuilderArgs args)
        {
            _correlationContextFactory.Create(args.CorrelationId?.Value, "correlation-id");
            _tenantBuilder.Build(args.TenantId);
        }
    }
}