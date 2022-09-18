using Microsoft.AspNetCore.Mvc;
using Todos.Persistence.Domain;
using Todos.Persistence.Repository;
using Todos.Todos.ViewModels;

namespace Todos.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodosController : ControllerBase
{
    private readonly IRepository<Todo> repository;
    private readonly ILogger<TodosController> logger;

    public TodosController(IRepository<Todo> repository, ILogger<TodosController> logger)
    {
        this.repository = repository;
        this.logger = logger;
    }

    [HttpPost]
    public async Task<IEnumerable<TodoViewModel>> Search([FromBody] Search search)
    {
        var todos = await this.repository.Search(search.SearchTerm);

        return todos
            .Select(x => new TodoViewModel(x.Id, x.Description, x.IsDone, x.CreatedAt))
            .ToArray();
    }
}
