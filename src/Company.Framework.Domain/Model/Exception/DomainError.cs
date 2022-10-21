using Company.Framework.Core.Error;

namespace Company.Framework.Domain.Model.Exception;

public record DomainError(string Code, string Message) : CoreError(Code, Message);