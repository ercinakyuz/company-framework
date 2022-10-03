using Company.Framework.ExampleApi.Data.Repository;
using Company.Framework.ExampleApi.Domain.Model.Aggregate.Converter;

namespace Company.Framework.ExampleApi.Domain.Model.Aggregate.OfWork;

public class ActionOfWork : IActionOfWork
{
    private readonly IActionRepository _actionRepository;

    private readonly IActionConverter _actionConverter;

    public ActionOfWork(IActionRepository actionRepository, IActionConverter actionConverter)
    {
        _actionRepository = actionRepository;
        _actionConverter = actionConverter;
    }

    public async Task InsertAsync(Action aggregate, CancellationToken cancellationToken)
    {
        await _actionRepository.InsertAsync(_actionConverter.Convert(aggregate));
    }

    public async Task UpdateAsync(Action aggregate, CancellationToken cancellationToken)
    {
        if (aggregate.HasAnyChanges())
            await _actionRepository.UpdateAsync(_actionConverter.Convert(aggregate));
    }

    public async Task DeleteAsync(Action aggregate, CancellationToken cancellationToken)
    {
        await _actionRepository.DeleteAsync(aggregate.Id.Value);
    }
}