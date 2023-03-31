using Company.Framework.Core.Exception;
using LanguageExt;
using LanguageExt.Common;
namespace Company.Framework.Core.Result.Extensions
{
    public static class ResultExtensions
    {
        public static TData IfFailTyped<TData>(this Result<TData> result, Func<StatelessCoreException, TData> failure)
        {
            return result.IfFail(exception => failure((StatelessCoreException)exception));
        }
    }

    public static class TryExtensions
    {
        public static TData IfFailTyped<TData>(this Try<TData> trial, Func<StatelessCoreException, TData> failure)
        {
            return trial.IfFail(exception => failure((StatelessCoreException)exception));
        }
    }
}
