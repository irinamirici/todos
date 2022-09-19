using System.ComponentModel.DataAnnotations;

namespace Todos.Todos.ViewModels;

/// <summary>
/// Class <c>SearchViewModel</c> provides search and pagination criteria
/// </summary>
public class SearchViewModel
{
    /// <summary>
    /// SearchTerm can be an empty string, to return all results, pages,
    /// or contains words that should be present in each searched item
    /// </summary>
    public string? SearchTerm { get; set; }

    /// <summary>
    /// What page to return, starting from 0
    /// </summary>
    public int Page { get; set; }

    /// <summary>
    /// Number of items per page. Should be under 50
    /// </summary>
    [Range(1, 50)]
    public int ItemsPerPage { get; set; } = 10;
}