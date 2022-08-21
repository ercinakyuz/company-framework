namespace Company.Framework.Aspect.Processors;

public abstract class PostProcessor<TArgs, TResult> : IPostProcessor
{
    protected abstract Task ProcessAsync(TArgs args, TResult result, CancellationToken cancellationToken);

    public async Task ExecuteAsync(object? args, object? result, CancellationToken cancellationToken)
    {
        var typedArgs = (TArgs)args!;
        TResult typedResult;
        if (result is Task task)
        {
            await task;
            try
            {
                typedResult = ((dynamic)task).Result;
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException(
                    $"Non generic result type not allowed, you should extend with {nameof(PostProcessor<TArgs>)}<{nameof(TArgs)}> instead.", exception);
            }
        }
        else
        {
            typedResult = (TResult)result!;
        }
        await ProcessAsync(typedArgs, typedResult, cancellationToken);
    }
}

public abstract class PostProcessor<TArgs> : IPostProcessor
{
    protected abstract Task ProcessAsync(TArgs aggregate, CancellationToken cancellationToken);

    public async Task ExecuteAsync(object? args, object? result, CancellationToken cancellationToken)
    {
        await ProcessAsync((TArgs)args!, cancellationToken);
    }
}