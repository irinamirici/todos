using Todos.Persistence.Domain;

namespace Todos.Persistence.Repository;

public interface IRepository<T> where T : new()
{
    Task<T?> Find(string id);
    Task<T> Create(T value);
    Task<T> Update(T value);
    Task Delete(string id);
    Task<Result<T>> Search(SearchCriteria criteria);
}