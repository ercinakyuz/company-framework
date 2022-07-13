namespace Company.Framework.ExampleApi.Domain.Model.Aggregate.OfWork;

public class ActionOfWork : IActionOfWork
{
    public async Task<Action> InsertAsync(Action aggregate, CancellationToken cancellationToken)
    {
        return await Task.FromResult(aggregate);
    }

    public async Task<Action> UpdateAsync(Action aggregate, CancellationToken cancellationToken)
    {
        return await Task.FromResult(aggregate);
    }

    public async Task<Action> DeleteAsync(Action aggregate, CancellationToken cancellationToken)
    {
        return await Task.FromResult(aggregate);
    }
}