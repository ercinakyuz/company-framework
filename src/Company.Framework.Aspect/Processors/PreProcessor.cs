namespace Company.Framework.Aspect.Processors;

public abstract class PreProcessor<TArgs> : IPreProcessor
{
    protected abstract Task ProcessAsync(TArgs envelope, CancellationToken cancellationToken);

    public async Task ExecuteAsync(object? args, CancellationToken cancellationToken)
    {
        var typedArgs = (TArgs)args!;
        await ProcessAsync(typedArgs, cancellationToken);
    }
}