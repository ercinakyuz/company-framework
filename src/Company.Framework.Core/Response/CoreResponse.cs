namespace Company.Framework.Core.Response;

public abstract record CoreResponse<TResult>(TResult Result);
