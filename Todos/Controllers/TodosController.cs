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
    public async Task<Paged<TodoViewModel>> Search([FromBody] SearchViewModel search)
    {
        var criteria = new SearchCriteria(search.SearchTerm, search.Page, search.ItemsPerPage);
        var todosResult = await this.repository.Search(criteria);

        var todosViewModels = todosResult.Data
            .Select(x => new TodoViewModel(x.Id, x.Description, x.IsDone, x.CreatedAt))
            .ToArray();

        return new Paged<TodoViewModel>(todosViewModels, todosResult.TotalItemsCount);
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
