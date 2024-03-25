using Microsoft.AspNetCore.Mvc;
using CsvHelper;
using System.Globalization;
using CsvHelper.Configuration;

namespace news_apis.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CountryListController : ControllerBase
{
    private readonly ILogger<CountryListController> _logger;
    private readonly CountryService _countryService;

    public CountryListController(ILogger<CountryListController> logger)
    {
        _logger = logger;
        _countryService = new CountryService();
    }

    [HttpGet]
    public IEnumerable<Country> Get()
    {
        return _countryService.GetCountries();
    }

}