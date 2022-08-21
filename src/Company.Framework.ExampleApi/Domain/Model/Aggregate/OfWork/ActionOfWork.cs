namespace Company.Framework.ExampleApi.Domain.Model.Aggregate.OfWork;

public class ActionOfWork : IActionOfWork
{
    public async Task InsertAsync(Action aggregate, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }

    public async Task UpdateAsync(Action aggregate, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(Action aggregate, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }
}