namespace Todos.Persistence.Domain;

public class Result<T>
{
    public long TotalItemsCount { get; init; }
    public IEnumerable<T> Data { get; init; } = null!;
}