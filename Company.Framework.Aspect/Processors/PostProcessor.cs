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
            typedResult = ((dynamic)task).Result;
        }
        else
        {
            typedResult = (TResult)result!;
        }
        await ProcessAsync(typedArgs, typedResult, cancellationToken);
    }
}