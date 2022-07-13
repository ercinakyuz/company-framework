namespace Company.Framework.Aspect.Processors;

public interface IPreProcessor
{
    Task ProcessAsync(object? args, CancellationToken cancellationToken);
}