namespace Todos.Persistence.Configuration;

public class SearchConfiguration
{
    public const string ConfigurationKey = "SearchConfiguration";

    public string Endpoint { get; init; } = string.Empty;
    public string ApiKey { get; init; } = string.Empty;
    public string IndexName { get; init; } = string.Empty;
}