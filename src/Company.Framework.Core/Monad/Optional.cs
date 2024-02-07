
namespace Company.Framework.Core.Monad;

public class Optional<TData>
{
    public static Optional<TData> Empty = new();

    public TData? Data { get; }

    private Optional() { }

    private Optional(TData data)
    {
        Data = data;
    }

    public static Optional<TData> Of(TData data)
    {
        if (data == null)
            throw new ArgumentNullException(nameof(data));
        return new Optional<TData>(data);
    }

    public static Optional<TData> OfNullable(TData? data)
    {
        return data == null ? Empty : new Optional<TData>(data);
    }

    public TData OrElse(Func<TData> function)
    {
        return IsEmpty() ? function() : Data!;
    }

    public TData OrElseThrow(Func<System.Exception> thrown)
    {
        return IsEmpty() ? throw thrown() : Data!;
    }

    public Optional<TProjection> Map<TProjection>(Func<TData, TProjection> mapped)
    {
        return IsEmpty() ? new Optional<TProjection>() : Optional<TProjection>.Of(mapped(Data!));
    }

    public bool IsEmpty()
    {
        return Data is null;
    }

    public bool IsPresent()
    {
        return !IsEmpty();
    }

    public void IfPresent(Action<TData> action)
    {
        if (IsPresent())
            action(Data!);
    }

    public void IfEmpty(Action action)
    {
        if (IsEmpty())
            action();
    }
}