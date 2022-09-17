namespace Todos.Persistence;
public class Todo
{
    public int Id { get; set; }

    public string Description { get; set; } = null!;

    public bool IsDone { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
}
