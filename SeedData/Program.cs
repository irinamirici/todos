using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Todos.SeedData;
using Todos.Persistence.Domain;
using Azure.Search.Documents;
using Azure.Search.Documents.Models;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using Azure;

Console.WriteLine("Import data");
var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
Console.WriteLine($"Env: {environmentName}");
var builder = new ConfigurationBuilder()
    .AddJsonFile($"appsettings.json", true, true)
    .AddJsonFile($"appsettings.{environmentName}.json", true, true)
    .AddEnvironmentVariables();

var configurationRoot = builder.Build();

var config = configurationRoot.GetSection(nameof(SearchConfiguration)).Get<SearchConfiguration>();

Console.WriteLine(config.Endpoint);
const string SEARCH_INDEX_NAME = "todos1";
Uri searchEndpointUri = new Uri(config.Endpoint);

SearchClient client = new SearchClient(
    searchEndpointUri,
    SEARCH_INDEX_NAME,
    new AzureKeyCredential(config.ApiKey));

SearchIndexClient clientIndex = new SearchIndexClient(
    searchEndpointUri,
    new AzureKeyCredential(config.ApiKey));

CreateIndexAsync(clientIndex).Wait();
BulkInsertAsync(client).Wait();

static async Task CreateIndexAsync(SearchIndexClient clientIndex)
{
    Console.WriteLine("Creating (or updating) search index");
    // TODO const string CORS = "https://portal.azure.com";
    var cors = new CorsOptions(new List<string> { "*" });
    cors.MaxAgeInSeconds = 300;
    FieldBuilder builder = new FieldBuilder();
    var index = new SearchIndex(SEARCH_INDEX_NAME)
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
        new Todo{ Id = Guid.NewGuid().ToString(), Description = "Feed the Cat", IsDone = false, CreatedAt = DateTimeOffset.UtcNow},
        new Todo{ Id = Guid.NewGuid().ToString(), Description = "Buy cat food", IsDone = false, CreatedAt = DateTimeOffset.UtcNow},
        new Todo{ Id = Guid.NewGuid().ToString(), Description = "Laundry", IsDone = false, CreatedAt = DateTimeOffset.UtcNow},
        new Todo{ Id = Guid.NewGuid().ToString(), Description = "Feed the Cat", IsDone = false, CreatedAt = DateTimeOffset.UtcNow},
        new Todo{ Id = Guid.NewGuid().ToString(), Description = "Buy cat food", IsDone = false, CreatedAt = DateTimeOffset.UtcNow},
        new Todo{ Id = Guid.NewGuid().ToString(), Description = "Laundry", IsDone = false, CreatedAt = DateTimeOffset.UtcNow}
    };

    Console.WriteLine("Uploading bulk data");
    _ = await client.UploadDocumentsAsync(todos);

    var docCount = (int)await client.GetDocumentCountAsync();

    Console.WriteLine($"Finished bulk inserting data. Count: {docCount}");
}