using CorrelationId;
using CorrelationId.Abstractions;
using Microsoft.Extensions.Options;

namespace Company.Framework.Correlation.Builder
{
    public class CorrelationContextBuilder : ICorrelationContextBuilder
    {
        private readonly ICorrelationContextFactory _correlationContextFactory;
        private readonly CorrelationIdOptions _options;

        public CorrelationContextBuilder(ICorrelationContextFactory correlationContextFactory, IOptions<CorrelationIdOptions> options)
        {
            _correlationContextFactory = correlationContextFactory;
            _options = options.Value;
        }

        public void Build(CorrelationId? id)
        {
            ArgumentNullException.ThrowIfNull(nameof(id));
            _correlationContextFactory.Create(id.Value, _options.RequestHeader);
        }
    }
}
