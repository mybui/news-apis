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
    public double Open { get; init; }
    public double High { get; init; }
    public double Low { get; init; }
    public double Close { get; init; }
}

public record Financials
{
    public DateTime Date { get; init; }
    public string? Cik { get; init; }
    public string? ReportedCurrency { get; init; }
    public long Revenue { get; init; }
    public long TotalAssets { get; init; }
    public long Cash { get; init; }
    public long CashAndEquivalent { get; init; }
    public long Inventory { get; init; }
    public long AccountsPayable { get; init; }
}
