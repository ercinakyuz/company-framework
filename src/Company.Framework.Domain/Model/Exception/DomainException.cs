using Company.Framework.Core.Exception;

namespace Company.Framework.Domain.Model.Exception
{
    public class DomainException : CoreException
    {
        public DomainException(ExceptionState state, DomainError error) : base(state, error)
        {
        }
    }
}
