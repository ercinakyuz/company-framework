using Company.Framework.Core.Error;

namespace Company.Framework.Core.Exception;

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