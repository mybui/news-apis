namespace news_apis;

public record FinanceInfo
{
    public Company? Company { get; init; }
    public Stock? Stock { get; init; }
    public Financials? Financials { get; init; }
}

public record Company
{
    public string? Ticker { get; init; }
    public string? Name { get; init; }
    public string? Currency { get; init; }
    public string? Exchange { get; init; }
}

public record Stock
{
    public decimal Open { get; set; }
    public decimal High { get; set; }
    public decimal Low { get; set; }
    public decimal Close { get; set; }
    public string? Currency { get; set; }
}

public record Financials
{
    public DateTime Date { get; init; }
    public string? Cik { get; init; }
    public string? Currency { get; set; }
    public long Revenue { get; set; }
    public long TotalAssets { get; set; }
    public long Cash { get; set; }
    public long CashAndEquivalent { get; set; }
    public long Inventory { get; set; }
    public long AccountsPayable { get; set; }

}