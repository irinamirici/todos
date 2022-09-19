using Todos.Persistence.Domain;

namespace Todos.Persistence.Repository;

public interface IRepository<T> where T : new()
{
    /// <summary>
    /// Searches an item by id
    /// </summary>
    /// <returns>
    /// Returns the item, or <c>null</c> if the id is not found
    /// </returns>
    Task<T?> Find(string id);
    Task<T> Create(T value);
    Task<T> Update(T value);
    Task Delete(string id);
    Task<Result<T>> Search(SearchCriteria criteria);
}