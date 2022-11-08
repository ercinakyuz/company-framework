using Company.Framework.Core.Error;
using Company.Framework.Core.Exception;

namespace Company.Framework.Application.Exception
{
    public class ApplicationException : StatefulCoreException
    {
        public ApplicationException(ExceptionState state, CoreError error, StatelessCoreException? innerException) : base(state, error, innerException)
        {
        }
        public ApplicationException(ExceptionState state, StatelessCoreException innerException) : base(state, "An application error occurred", innerException)
        {
        }
    }
}