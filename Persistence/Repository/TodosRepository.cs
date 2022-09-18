using Todos.Persistence.Domain;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents;
using Azure.Search.Documents.Models;
using Todos.Persistence.Configuration;
using Microsoft.Extensions.Options;

namespace Todos.Persistence.Repository;

public class TodosRepository : IRepository<Todo>
{
    private readonly SearchClient searchClient;
    private readonly SearchConfiguration searchConfiguration;

    public TodosRepository(SearchIndexClient searchIndexClient, IOptions<SearchConfiguration> confOptions)
    {
        searchConfiguration = confOptions.Value;
        searchClient = searchIndexClient.GetSearchClient(searchConfiguration.IndexName);
    }

    public async Task<Todo> Find(string id)
    {
        var todo = await this.searchClient.GetDocumentAsync<Todo>(id);

        return todo;
    }

    public async Task<IEnumerable<Todo>> Search(string? searchTerm)
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
        var result = await this.searchClient.SearchAsync<Todo>(searchTerm, searchOptions);

        var todos = result.Value.GetResults().Select(x => x.Document).ToArray();

        return todos;
    }
}