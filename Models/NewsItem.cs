namespace news_apis;

public record NewsItem
{
    public string? Title { get; set; }
    public string? Snippet { get; set; }
    public string? Url { get; set; }
    public DateTime? PublishedAt { get; set; }
    public string? Language { get; set; }
}