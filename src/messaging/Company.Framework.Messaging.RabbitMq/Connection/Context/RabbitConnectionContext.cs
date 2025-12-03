using RabbitMQ.Client;

namespace Company.Framework.Messaging.RabbitMq.Connection.Context;

public record RabbitConnectionContext(string BusName, IConnectionFactory ConnectionFactory, string? Nodes) : IRabbitConnectionContext
{
    private IConnection? _connection;

    public async Task<TConnection> ResolveAsync<TConnection>(CancellationToken cancellationToken)
    {
        Validate<TConnection>();
        _connection = await GetOrCreateAsync(cancellationToken);
        return (TConnection)_connection;
    }

    private void Validate<TConnection>()
    {
        if (typeof(TConnection) != typeof(IConnection))
            throw new InvalidOperationException("ConnectionContext type is not valid");
    }

    private Task<IConnection> GetOrCreateAsync(CancellationToken cancellationToken)
    {
        if (_connection is { IsOpen: true }) return Task.FromResult(_connection);
        return Nodes == null
            ? ConnectionFactory.CreateConnectionAsync(cancellationToken)
            : ConnectionFactory.CreateConnectionAsync(Nodes.Split(";"), cancellationToken);
    }
}