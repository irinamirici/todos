namespace Todos.Persistence.Configuration;

public class SearchConfiguration
{
    public const string ConfigurationKey = "SearchConfiguration";

    public string Endpoint { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
    public string IndexName { get; set; } = string.Empty;
}