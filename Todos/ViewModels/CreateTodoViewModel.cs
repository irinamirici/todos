using System.ComponentModel.DataAnnotations;

namespace Todos.Todos.ViewModels;

public record CreateTodoViewModel([Required][MaxLength(25)] string Description);