namespace Company.Framework.Core.Error
{
    public record CoreError(string Code, string Message, string? UserMessage = default);
}