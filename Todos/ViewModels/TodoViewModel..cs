namespace Todos.Todos.ViewModels;

public record TodoViewModel(string Id, string Description, bool IsDone, DateTimeOffset CreatedAt)
{

}