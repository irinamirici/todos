using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Todos.Persistence.Domain;
using Todos.Persistence.Configuration;
using Azure.Search.Documents;
using Azure.Search.Documents.Models;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using Azure;

// quick and dirty way to import some data and create an index
Console.WriteLine("Import data");
var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
Console.WriteLine($"Env: {environmentName}");
var builder = new ConfigurationBuilder()
    .AddJsonFile($"appsettings.json", true, true)
    .AddJsonFile($"appsettings.{environmentName}.json", true, true)
    .AddEnvironmentVariables();

var configurationRoot = builder.Build();

var config = configurationRoot.GetSection(nameof(SearchConfiguration)).Get<SearchConfiguration>();

Uri searchEndpointUri = new Uri(config.Endpoint);

SearchIndexClient clientIndex = new SearchIndexClient(
    searchEndpointUri,
    new AzureKeyCredential(config.ApiKey));

CreateIndexAsync(clientIndex, config.IndexName).Wait();
BulkInsertAsync(clientIndex.GetSearchClient("todos1")).Wait();

static async Task CreateIndexAsync(SearchIndexClient clientIndex, string indexName)
{
    Console.WriteLine("Creating (or updating) search index");
    // TODO const string CORS = "https://portal.azure.com";
    var cors = new CorsOptions(new List<string> { "*" });
    cors.MaxAgeInSeconds = 300;
    FieldBuilder builder = new FieldBuilder();
    var index = new SearchIndex(indexName)
    {
        Fields = builder.Build(typeof(Todo)),
        CorsOptions = cors
    };
    var result = await clientIndex.CreateOrUpdateIndexAsync(index);

    Console.WriteLine(result);
}

static async Task BulkInsertAsync(SearchClient client)
{
    var todos = new List<Todo>{
        new Todo{ Id = Guid.NewGuid().ToString(), Description = "Test 1", IsDone = false, CreatedAt = DateTimeOffset.UtcNow},
        new Todo{ Id = Guid.NewGuid().ToString(), Description = "Test 2", IsDone = false, CreatedAt = DateTimeOffset.UtcNow},
        new Todo{ Id = Guid.NewGuid().ToString(), Description = "Dishes", IsDone = false, CreatedAt = DateTimeOffset.UtcNow},
        new Todo{ Id = Guid.NewGuid().ToString(), Description = "test 3", IsDone = false, CreatedAt = DateTimeOffset.UtcNow},
        new Todo{ Id = Guid.NewGuid().ToString(), Description = "4 test", IsDone = false, CreatedAt = DateTimeOffset.UtcNow},
        new Todo{ Id = Guid.NewGuid().ToString(), Description = "5 test", IsDone = false, CreatedAt = DateTimeOffset.UtcNow}
    };

    Console.WriteLine("Uploading data");
    _ = await client.UploadDocumentsAsync(todos);

    var docCount = (int)await client.GetDocumentCountAsync();

    Console.WriteLine($"Finished inserting data. Count: {docCount}");
}