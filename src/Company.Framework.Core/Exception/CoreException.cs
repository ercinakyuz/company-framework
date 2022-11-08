using Company.Framework.Core.Error;

namespace Company.Framework.Core.Exception
{
    public abstract class StatefulCoreException : System.Exception
    {
        public ExceptionState State { get; }

        public CoreError? Error { get; }

        public CoreError ActualError
        {
            get
            {
                if (Error != default)
                    return Error;
                var innerException = (StatelessCoreException)InnerException!;
                return new CoreError(innerException.Code, innerException.Message, innerException.UserMessage);

            }
        }

        protected StatefulCoreException(ExceptionState state, string message, StatelessCoreException innerException) : base(message, innerException)
        {
            State = state;
        }

        protected StatefulCoreException(ExceptionState state, CoreError error, StatelessCoreException? innerException = default) : base(error.Message, innerException)
        {
            State = state;
            Error = error;
        }
    }

    public abstract class StatelessCoreException : System.Exception
    {
        public string Code { get; }

        public string? UserMessage { get; }

        protected StatelessCoreException(CoreError error, StatelessCoreException? innerException = default) : base(error.Message, innerException)
        {
            Code = error.Code;
            UserMessage = error.UserMessage;
        }

        public override string ToString()
        {
            return $"{GetType().FullName}: {Message}, ErrorCode: {Code}, UserMessage: {UserMessage}";
        }
    }
}
