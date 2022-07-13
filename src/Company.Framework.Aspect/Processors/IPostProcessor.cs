namespace Company.Framework.Aspect.Processors;

public interface IPostProcessor
{
    Task ExecuteAsync(object? args, object? result, CancellationToken cancellationToken);
}