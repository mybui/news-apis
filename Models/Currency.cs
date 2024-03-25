using CsvHelper.Configuration.Attributes;

namespace news_apis;

public class Currency
{
    [Name("Currency Code")]
    public string? Code { get; init; }

    [Name("Currency Name")]
    public string? Name { get; init; }

    [Name("Country")]
    public string? Country { get; init; }
}