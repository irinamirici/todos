using System.ComponentModel.DataAnnotations;

namespace Todos.Todos.ViewModels;

public record Paged<T>([Required] IEnumerable<T> Data, [Required] long TotalCount);
