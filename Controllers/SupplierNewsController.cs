using Microsoft.AspNetCore.Mvc;
using news_apis;
namespace news_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SupplierNewsController : ControllerBase
{
    private readonly ILogger<SupplierNewsController> _logger;
    private readonly IConfiguration Configuration;
    private readonly NewsService _newsService;

    public SupplierNewsController(ILogger<SupplierNewsController> logger, IConfiguration configuration)
    {
        _logger = logger;
        Configuration = configuration;
        _newsService = new(Configuration);
    }

    [HttpGet]
    public IEnumerable<NewsItem> Get(string supplierName, string? country, string? language, int pageNumber = 1)
    {
        List<NewsItem> newsItems = _newsService.CallAllApis(supplierName, country, language, pageNumber);
        return newsItems;
    }

}