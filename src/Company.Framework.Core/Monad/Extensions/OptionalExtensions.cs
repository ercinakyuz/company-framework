using Company.Framework.Core.Error;

namespace Company.Framework.Core.Monad.Extensions;

public static class OptionalExtensions
{
    public static Result<TData> ToResult<TData>(this Optional<TData> optional, Func<CoreError> failure)
    {
        return optional.IsEmpty() ? Result<TData>.OfFail(failure()) : Result<TData>.OfSuccess(optional.Data!);
    }
}