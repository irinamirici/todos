namespace Todos.Persistence.Domain;

public class SearchCriteria
{
    /// <summary>
    /// Separators to split words in the searched phrase by
    /// </summary>
    private readonly char[] separators = new[] { ' ', ',', ';', '.' };
    private const string FUZZY_WORD_JOIN = " AND ";
    private const string FUZZY_SEARCH_INDICATOR = "~";

    /// <summary>
    /// Performs an empty fuzzy search, 
    /// i.e. it retrieves all results, as if no search term was provided
    /// </summary>
    private const string EMPTY_SEARCH = "*";
    public SearchCriteria(string? search, int page, int itemsPerPage)
    {
        this.SearchText = search;
        this.Page = page;
        this.ItemsPerPage = itemsPerPage;
    }

    public string? SearchText { get; init; }
    public int Page { get; init; }
    public int ItemsPerPage { get; init; }

    /// <summary>
    /// Builds a full Lucene syntax search expression, performing Fuzzy search for each word
    /// Search distance is 2 (default)
    /// </summary>
    public string FuzzySearchTerm
    {
        get
        {
            if (string.IsNullOrWhiteSpace(this.SearchText))
            {
                return EMPTY_SEARCH; // get everything
            }

            // split words and add fuzzy search indicator (~) for each
            var words = this.SearchText.Split(this.separators,
                StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            return string.Join(FUZZY_WORD_JOIN, words.Select(x => x + FUZZY_SEARCH_INDICATOR));
        }
    }

    public int Skip => this.Page * this.ItemsPerPage;
}