using Company.Framework.Core.Error;
using Company.Framework.Core.Exception;

namespace Company.Framework.Application.Exception
{
    public class ApplicationException : StatefulCoreException
    {
        public ApplicationException(ExceptionState state, CoreError rootCause) : base(state, rootCause)
        {
        }
        //public ApplicationException(ExceptionState state, StatelessCoreException innerException) : base(state, "An application rootCause occurred", innerException)
        //{
        //}
    }
}