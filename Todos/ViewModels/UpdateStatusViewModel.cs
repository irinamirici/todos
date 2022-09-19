using System.ComponentModel.DataAnnotations;

namespace Todos.Todos.ViewModels;

public record UpdateStatusViewModel([Required] bool IsDone);