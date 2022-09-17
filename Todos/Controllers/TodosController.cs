using Microsoft.AspNetCore.Mvc;
using Todos.Persistence;
using Todos.Todos.ViewModels;

namespace Todos.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodosController : ControllerBase
{
    private static readonly Todo[] SampleTodos = new Todo[]
    {
        new Todo{ Id = Guid.NewGuid().ToString(), Description = "Feed the Cat", IsDone = false, CreatedAt = DateTimeOffset.UtcNow},
        new Todo{ Id = Guid.NewGuid().ToString(), Description = "Buy cat food", IsDone = false, CreatedAt = DateTimeOffset.UtcNow},
        new Todo{ Id = Guid.NewGuid().ToString(), Description = "Laundry", IsDone = false, CreatedAt = DateTimeOffset.UtcNow},
        new Todo{ Id = Guid.NewGuid().ToString(), Description = "Feed the Cat", IsDone = false, CreatedAt = DateTimeOffset.UtcNow},
        new Todo{ Id = Guid.NewGuid().ToString(), Description = "Buy cat food", IsDone = false, CreatedAt = DateTimeOffset.UtcNow},
        new Todo{ Id = Guid.NewGuid().ToString(), Description = "Laundry", IsDone = false, CreatedAt = DateTimeOffset.UtcNow}
    };


    private readonly ILogger<TodosController> _logger;

    public TodosController(ILogger<TodosController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IEnumerable<TodoViewModel> Get()
    {
        return SampleTodos
            .Select(x => new TodoViewModel(x.Id, x.Description, x.IsDone, x.CreatedAt))
            .ToArray();
    }
}
