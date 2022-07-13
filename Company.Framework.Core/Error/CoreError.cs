namespace Company.Framework.Core.Error
{
    public abstract record CoreError(string Code, string Message, string? UserMessage);
}