namespace Todos.Todos.ViewModels;

public record TodoViewModel(int Id, string Description, bool IsDone, DateTimeOffset CreatedAt)
{

}