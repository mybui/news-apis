using Microsoft.AspNetCore.Mvc;
using CsvHelper;
using System.Globalization;
using CsvHelper.Configuration;
using news_apis;

namespace news_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LanguageListController : ControllerBase
{
    private readonly ILogger<LanguageListController> _logger;
    private readonly LanguageService _languageService;

    public LanguageListController(ILogger<LanguageListController> logger)
    {
        _logger = logger;
        _languageService = new LanguageService();
    }

    [HttpGet]
    public IEnumerable<Language> Get()
    {
        return _languageService.GetLanguages();
    }

}
