namespace Todos.Persistence.Repository;

public interface IRepository<T> where T : new()
{
    Task<T> Find(string id);
    Task<IEnumerable<T>> Search(string? searchTerm);
}