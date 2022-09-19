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

    public Task<Todo> Create(Todo todo)
    {
        return UploadDocument(todo);
    }

    public Task<Todo> Update(Todo todo)
    {
        return UploadDocument(todo);
    }

    public async Task Delete(string id)
    {
        await this.searchClient.DeleteDocumentsAsync("Id", new[] { id }, throwOnAnyErrorOptions);
    }

    /// <summary>
    /// Performs a Fuzzy search for each word in the search criteria, 
    /// and pages the result
    /// </summary>
    /// <param name="criteria">the search and pagination criteria</param>
    /// <returns>
    /// Returns the search result, including total item count
    /// </returns>
    public async Task<Result<Todo>> Search(SearchCriteria criteria)
    {
        var searchOptions = new SearchOptions
        {
            QueryType = SearchQueryType.Full, // Lucene syntax
            SearchMode = SearchMode.All,
            SearchFields = { "Description" },
            IncludeTotalCount = true,
            Skip = criteria.Skip,
            Size = criteria.ItemsPerPage
        };
        var result = await this.searchClient.SearchAsync<Todo>(criteria.FuzzySearchTerm, searchOptions);

        var todosResult = new Result<Todo>
        {
            TotalItemsCount = result.Value.TotalCount!.Value,
            Data = result.Value.GetResults().Select(x => x.Document).ToArray()
        };

        return todosResult;
    }

    private async Task<Todo> UploadDocument(Todo todo)
    {
        await this.searchClient.UploadDocumentsAsync<Todo>(new[] { todo }, throwOnAnyErrorOptions);

        return todo;
    }
}