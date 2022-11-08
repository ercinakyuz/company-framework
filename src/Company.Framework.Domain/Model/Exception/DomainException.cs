using Company.Framework.Core.Error;
using Company.Framework.Core.Exception;

namespace Company.Framework.Domain.Model.Exception
{
    public class DomainException : StatefulCoreException
    {
        public DomainException(ExceptionState state, DomainError error) : base(state, error)
        {
        }
    }

    public class AggregateBuilderException : StatelessCoreException
    {
        public AggregateBuilderException(CoreError error) : base(error)
        {
        }
    }
}
