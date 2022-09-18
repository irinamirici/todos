using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;

namespace Todos.Persistence.Domain;
public class Todo
{
    [SimpleField(IsKey = true)]
    public string Id { get; set; } = null!;

    [SearchableField(IsSortable = false, AnalyzerName = LexicalAnalyzerName.Values.EnLucene)]
    public string Description { get; set; } = null!;

    // TODO - learn what IsFacetable does
    [SimpleField(IsFacetable = true, IsFilterable = true, IsSortable = true)]
    public bool IsDone { get; set; }

    [SimpleField(IsFilterable = true, IsSortable = true)]
    public DateTimeOffset CreatedAt { get; set; }

    public void MarkAsDone()
    {
        IsDone = true;
    }
}
