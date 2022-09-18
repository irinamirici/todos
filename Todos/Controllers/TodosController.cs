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
    public async Task<IActionResult> Create([FromBody] CreateTodoViewModel model)
    {
        var todo = await this.repository.Create(new Todo
        {
            Description = model.Description
        });

        // It's not returning Created result (201),
        // because we don't yet have a route to get one todo by id
        return Ok(todo);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        logger.LogDebug($"Delete {id}");

        try
        {
            await this.repository.Delete(id);
        }
        catch (Exception)
        {
            return BadRequest($"Could not delete {id}");
        }

        return Ok();
    }

    [HttpPost("search")]
    public async Task<IEnumerable<TodoViewModel>> Search([FromBody] Search search)
    {
        var todos = await this.repository.Search(search.SearchTerm);

        return todos
            .Select(x => new TodoViewModel(x.Id, x.Description, x.IsDone, x.CreatedAt))
            .ToArray();
    }

    [HttpPost("{id}/done")]
    public async Task<IActionResult> MarkAsDone(string id)
    {
        var todo = await this.repository.Find(id);
        if (todo == null)
        {
            return NotFound($"{id} not found");
        }
        if (todo.IsDone)
        {
            return Ok(todo);
        }

        todo.MarkAsDone();
        await this.repository.Update(todo);

        return Ok(todo);
    }
}
