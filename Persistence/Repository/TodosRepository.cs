using Todos.Persistence.Domain;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents;
using Azure.Search.Documents.Models;

namespace Todos.Persistence.Repository;

public class TodosRepository : IRepository<Todo>
{
    private readonly SearchIndexClient searchIndexClient;

    public TodosRepository(SearchIndexClient searchIndexClient)
    {
        this.searchIndexClient = searchIndexClient;
    }

    public async Task<Todo> Find(string id)
    {
        var todo = await this.searchIndexClient
            .GetSearchClient("todos1")
            .GetDocumentAsync<Todo>(id);

        return todo;
    }

    public async Task<IEnumerable<Todo>> Search(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            searchTerm = "*"; // get everything
        }
        else
        {
            searchTerm = searchTerm + "~"; // fuzzy
        }

        var searchOptions = new SearchOptions
        {
            QueryType = SearchQueryType.Full, // Lucene syntax
            SearchMode = SearchMode.All,
            SearchFields = { "Description" }
        };
        var result = await this.searchIndexClient
            .GetSearchClient("todos1")
            .SearchAsync<Todo>(searchTerm, searchOptions);

        var todos = result.Value.GetResults().Select(x => x.Document).ToArray();

        return todos;
    }
}