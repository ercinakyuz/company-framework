using Company.Framework.Core.Identity;

namespace Company.Framework.Messaging
{
    public class EnvelopeCoreId : CoreId<EnvelopeCoreId,Guid>
    {
        public EnvelopeCoreId(Guid value) : base(value)
        {
        }

        public EnvelopeCoreId(IdGenerationType generationType) : base(generationType)
        {
        }
    }
}
