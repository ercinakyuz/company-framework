using Company.Framework.Core.Identity;

namespace Company.Framework.Correlation
{
    public class CorrelationId : CoreId<CorrelationId, string>
    {
        public CorrelationId(string value) : base(value)
        {
        }
        public CorrelationId(IdGenerationType generationType) : base(generationType)
        {
        }
    }
}