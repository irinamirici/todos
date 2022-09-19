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

    /// <summary>
    /// Creates a new TODO item
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTodoViewModel model)
    {
        var todo = await this.repository.Create(Todo.FromDescription(model.Description));

        // Not returning Created result (201),
        // because we don't yet have a route to get one todo by id
        return Ok(todo);
    }

    /// <summary>
    /// Deletes a TODO item by Id. If the Id doesn't exist, no error is returned.
    /// </summary>
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

    /// <summary>
    /// Returns paged TODO items, based on the search criteria and paging informatiom.
    /// To iterate through all items, use an empty SearchTerm
    /// </summary>
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

    /// <summary>
    /// Updates a TODO's status
    /// </summary>
    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateStatus(string id, [FromBody] UpdateStatusViewModel model)
    {
        var todo = await this.repository.Find(id);
        if (todo == null)
        {
            return NotFound($"{id} not found");
        }

        if (todo.IsDone == model.IsDone)
        {
            // nothing to update
            return Ok(todo);
        }

        todo.IsDone = model.IsDone;
        await this.repository.Update(todo);

        return Ok(todo);
    }
}
