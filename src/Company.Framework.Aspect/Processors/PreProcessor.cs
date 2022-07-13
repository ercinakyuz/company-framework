namespace Company.Framework.Aspect.Processors;

public abstract class PreProcessor<TArgs> : IPreProcessor
{
    protected abstract Task ProcessAsync(TArgs args, CancellationToken cancellationToken);

    public async Task ProcessAsync(object? args, CancellationToken cancellationToken)
    {
        var typedArgs = (TArgs)args!;
        await ProcessAsync(typedArgs, cancellationToken);
    }
}