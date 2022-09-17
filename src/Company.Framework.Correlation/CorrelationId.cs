using Company.Framework.Core.Identity;

namespace Company.Framework.Correlation
{
    public class CorrelationCoreId : CoreId<CorrelationCoreId, string>
    {
        public CorrelationCoreId(string value) : base(value)
        {
        }
        public CorrelationCoreId(IdGenerationType generationType) : base(generationType)
        {
        }
    }
}