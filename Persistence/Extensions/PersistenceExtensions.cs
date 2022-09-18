using Microsoft.Extensions.DependencyInjection;
using Azure;
using Microsoft.Extensions.Azure;

using Todos.Persistence.Domain;
using Todos.Persistence.Repository;
using Todos.Persistence.Configuration;

namespace Todos.Persistence.Extensions;

public static class PersistenceExtensions
{
    public static void AddPersistence(this IServiceCollection collection, SearchConfiguration searchConfiguration)
    {
        collection.AddAzureClients(builder =>
        {
            builder.AddSearchIndexClient(new
                Uri(searchConfiguration.Endpoint), new
                AzureKeyCredential(searchConfiguration.ApiKey));
        });
        collection.AddScoped(typeof(IRepository<Todo>), typeof(TodosRepository));
    }
}