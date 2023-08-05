namespace Company.Framework.Aspect.Processors;

public interface IPreProcessor
{
    Task ExecuteAsync(object? args, CancellationToken cancellationToken);
}