using System.ComponentModel.DataAnnotations;

namespace Todos.Todos.ViewModels;

public record TodoViewModel([Required] string Id,
    [Required] string Description,
    [Required] bool IsDone,
    [Required] DateTimeOffset CreatedAt);