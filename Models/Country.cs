using CsvHelper.Configuration.Attributes;

namespace news_apis;

public class Country
{
    [Name("Country")]
    public string? Name { get; set; }

    [Name("Alpha-2 code")]
    public string? Alpha2 { get; set; }

    [Name("Alpha-3 code")]
    public string? Alpha3 { get; set; }
}