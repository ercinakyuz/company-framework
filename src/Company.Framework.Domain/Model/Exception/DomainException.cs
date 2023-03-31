using Company.Framework.Core.Error;
using Company.Framework.Core.Exception;

namespace Company.Framework.Domain.Model.Exception
{
    public class DomainException : StatelessCoreException
    {
        public DomainException(CoreError error) : base(error)
        {
        }
    }
}
