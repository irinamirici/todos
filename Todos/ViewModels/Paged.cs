namespace Todos.Todos.ViewModels;

public record Paged<T>(IEnumerable<T> Data, long TotalCount);
