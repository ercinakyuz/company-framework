namespace Company.Framework.Api.Models
{
    public abstract class CoreApiResponse<TResult>
    {
        public TResult Result { get; init; }
    }
}
