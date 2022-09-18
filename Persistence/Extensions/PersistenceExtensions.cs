using Microsoft.Extensions.DependencyInjection;
using Todos.Persistence.Domain;
using Todos.Persistence.Repository;

namespace Todos.Persistence.Extensions;

public static class PersistenceExtensions
{
    public static void AddRepositories(this IServiceCollection collection)
    {
        collection.AddScoped(typeof(IRepository<Todo>), typeof(TodosRepository));
    }
}