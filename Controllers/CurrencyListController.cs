using Microsoft.AspNetCore.Mvc;
using CsvHelper;
using System.Globalization;
using CsvHelper.Configuration;

namespace news_apis.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CurrencyListController : ControllerBase
{
    private readonly ILogger<CurrencyListController> _logger;
    private readonly IConfiguration Configuration;
    private readonly CurrencyService _currencyService;

    public CurrencyListController(ILogger<CurrencyListController> logger, IConfiguration configuration)
    {
        _logger = logger;
        Configuration = configuration;
        _currencyService = new CurrencyService(Configuration);
    }

    [HttpGet]
    public IEnumerable<Currency> Get()
    {
        return _currencyService.GetCurrencies();
    }

}