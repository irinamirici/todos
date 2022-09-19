using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;

namespace Todos.Persistence.Domain;
public class Todo
{
    [SimpleField(IsKey = true)]
    public string Id { get; init; } = null!;

    [SearchableField(IsSortable = false, AnalyzerName = LexicalAnalyzerName.Values.EnLucene)]
    public string Description { get; init; } = null!;

    [SimpleField(IsFilterable = true, IsSortable = true)]
    public bool IsDone { get; set; }

    [SimpleField(IsFilterable = true, IsSortable = true)]
    public DateTimeOffset CreatedAt { get; init; }

    public static Todo FromDescription(string description)
    {
        return new Todo
        {
            Id = Guid.NewGuid().ToString(),
            Description = description,
            CreatedAt = DateTimeOffset.UtcNow
        };
    }
}
