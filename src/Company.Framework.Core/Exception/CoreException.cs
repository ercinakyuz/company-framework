using Company.Framework.Core.Error;

namespace Company.Framework.Core.Exception
{
    public abstract class CoreException : System.Exception
    {
        public ExceptionState State { get; }

        public string Code { get; }

        public string? UserMessage { get; }

        protected CoreException(ExceptionState state, CoreError error) : base(error.Message)
        {
            State = state;
            Code = error.Code;
            UserMessage = error.UserMessage;
        }
    }
}
