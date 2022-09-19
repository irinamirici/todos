namespace Todos.Persistence.Domain;

public class SearchCriteria
{
    public SearchCriteria(string? search, int page, int itemsPerPage)
    {
        this.SearchText = search;
        this.Page = page;
        this.ItemsPerPage = itemsPerPage;
    }

    public string? SearchText { get; init; }
    public int Page { get; init; }
    public int ItemsPerPage { get; init; }

    public string FuzzySearchTerm
    {
        get
        {
            if (string.IsNullOrWhiteSpace(this.SearchText))
            {
                return "*"; // get everything
            }

            // TODO - split words and add fuzzy for each
            return this.SearchText + "~"; // fuzzy
        }
    }

    public int Skip => this.Page * this.ItemsPerPage;
}