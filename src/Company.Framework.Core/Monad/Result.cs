using Company.Framework.Core.Error;

namespace Company.Framework.Core.Monad
{
    public class Result<TData> : Result
    {
        public TData? Data { get; }

        private Result(TData data)
        {
            Data = data;
        }

        private Result(CoreError error) : base(error)
        {
        }

        public static Result<TData> OfSuccess(TData data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            return new Result<TData>(data);
        }

        public new static Result<TData> OfFail(CoreError error)
        {
            return new Result<TData>(error);
        }

        public new TData ThrowOnFail(Func<CoreError, System.Exception> failure)
        {
            if (!Success)
                throw failure(Error!);
            return Data!;
        }

        public new TData ThrowOnFail(Func<System.Exception> failure)
        {
            if (!Success)
                throw failure();
            return Data!;
        }

        public TData OnFail(Func<CoreError, TData> failure)
        {
            return Success ? Data! : failure(Error!);
        }
    }

    public class Result
    {
        public bool Success { get; } = true;

        public CoreError? Error { get; }

        protected Result() { }

        protected Result(CoreError error)
        {
            Error = error;
            Success = false;
        }

        public static Result OfFail(CoreError error)
        {
            if (error == null)
                throw new ArgumentNullException(nameof(error));
            return new Result(error);
        }

        public static Result OfSuccess()
        {
            return new Result();
        }

        public void ThrowOnFail(Func<CoreError, System.Exception> failure)
        {
            if (!Success)
                throw failure(Error!);
        }

        public void ThrowOnFail(Func<System.Exception> failure)
        {
            if (!Success)
                throw failure();
        }

        public void IfFailed(Action<CoreError> failure)
        {
            if (!Success)
                failure(Error!);
        }

        public void IfSucceeded(Action succeeded)
        {
            if (!Success)
                succeeded();
        }
    }
}
