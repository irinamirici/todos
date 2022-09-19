namespace Todos.Persistence.Domain;

public class Result<T>
{
    public long TotalItemsCount { get; set; }
    public IEnumerable<T> Data { get; set; } = null!;
}