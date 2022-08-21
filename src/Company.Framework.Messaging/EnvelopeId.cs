using Company.Framework.Core.Identity;

namespace Company.Framework.Messaging
{
    public class EnvelopeId : CoreId<EnvelopeId,Guid>
    {
        public EnvelopeId(Guid value) : base(value)
        {
        }

        public EnvelopeId(IdGenerationType generationType) : base(generationType)
        {
        }
    }
}
