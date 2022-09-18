using Todos.Persistence.Domain;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents;
using Azure.Search.Documents.Models;
using Todos.Persistence.Configuration;
using Microsoft.Extensions.Options;

namespace Todos.Persistence.Repository;

public class TodosRepository : IRepository<Todo>
{
    private readonly IndexDocumentsOptions throwOnAnyErrorOptions = new IndexDocumentsOptions
    {
        ThrowOnAnyError = true
    };

    private readonly SearchClient searchClient;
    private readonly SearchConfiguration searchConfiguration;

    public TodosRepository(SearchIndexClient searchIndexClient, IOptions<SearchConfiguration> confOptions)
    {
        searchConfiguration = confOptions.Value;
        searchClient = searchIndexClient.GetSearchClient(searchConfiguration.IndexName);
    }

    public async Task<Todo?> Find(string id)
    {
        try
        {
            return await this.searchClient.GetDocumentAsync<Todo>(id);
        }
        catch
        {
            return null;
        }
    }

    public async Task<Todo> Create(Todo todo)
    {
        todo.Id = Guid.NewGuid().ToString();
        todo.CreatedAt = DateTimeOffset.UtcNow;
        await this.searchClient.UploadDocumentsAsync<Todo>(new[] { todo }, throwOnAnyErrorOptions);

        return todo;
    }

    public async Task<Todo> Update(Todo todo)
    {
        await this.searchClient.UploadDocumentsAsync<Todo>(new[] { todo }, throwOnAnyErrorOptions);

        return todo;
    }

    public async Task Delete(string id)
    {
        await this.searchClient.DeleteDocumentsAsync("Id", new[] { id }, throwOnAnyErrorOptions);
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