using Company.Framework.Core.Id.Implementations;

namespace Company.Framework.Correlation
{
    public record CorrelationId(string? Value) : IdOfString<CorrelationId>(Value);
}