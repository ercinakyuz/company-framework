namespace Company.Framework.Aspect.Processors;

public abstract class PostProcessor<TArgs, TResult> : IPostProcessor where TArgs : class where TResult : class
{
    protected abstract Task ProcessAsync(TArgs args, TResult result, CancellationToken cancellationToken);

    public async Task ExecuteAsync(object? args, object? result, CancellationToken cancellationToken)
    {
        var typedArgs = args as TArgs;
        var typedResult = result as TResult;
        if (typedArgs is not null && typedResult is not null)
            await ProcessAsync(typedArgs, typedResult, cancellationToken);

    }
}

public abstract class PostProcessor<TArgs> : IPostProcessor where TArgs : class
{
    protected abstract Task ProcessAsync(TArgs aggregate, CancellationToken cancellationToken);

    public async Task ExecuteAsync(object? args, object? result, CancellationToken cancellationToken)
    {
        var typedArgs = args as TArgs;
        if (typedArgs is not null)
            await ProcessAsync((TArgs)args!, cancellationToken);
    }
}