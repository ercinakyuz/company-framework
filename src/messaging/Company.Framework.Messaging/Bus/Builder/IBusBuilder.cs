using Microsoft.Extensions.DependencyInjection;

namespace Company.Framework.Messaging.Bus.Builder;

public interface IBusBuilder
{
    IServiceCollection ServiceCollection { get; }
}