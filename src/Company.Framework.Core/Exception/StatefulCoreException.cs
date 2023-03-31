using Company.Framework.Core.Error;

namespace Company.Framework.Core.Exception
{
    public abstract class StatefulCoreException : System.Exception
    {
        public ExceptionState State { get; }

        public CoreError? RootCause { get; }

        public CoreError ActualError
        {
            get
            {
                if (RootCause != default)
                    return RootCause;
                var innerException = (StatelessCoreException)InnerException!;
                return new CoreError(innerException.Code, innerException.Message, innerException.UserMessage);

            }
        }

        protected StatefulCoreException(ExceptionState state, string message, StatelessCoreException innerException) : base(message, innerException)
        {
            State = state;
        }

        protected StatefulCoreException(ExceptionState state, CoreError rootCause, StatelessCoreException? innerException = default) : base(rootCause.Message, innerException)
        {
            State = state;
            RootCause = rootCause;
        }
    }
}
