using Company.Framework.Core.Tenancy.Models;

namespace Company.Framework.Infrastructure.Application.Context.Builder.Args
{
    public record ApplicationContextBuilderArgs(Correlation.CorrelationId? CorrelationId, TenantId? TenantId);
}