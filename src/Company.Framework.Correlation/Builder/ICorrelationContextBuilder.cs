namespace Company.Framework.Correlation.Builder
{
    public interface ICorrelationContextBuilder
    {
        void Build(CorrelationId? id);
    }
}
