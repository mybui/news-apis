using Microsoft.AspNetCore.Mvc;
namespace news_apis.Controllers;

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
    public async Task<IActionResult> Get(string supplierName, string? country, string? language, int pageNumber = 1)
    {
        try
        {
            List<NewsItem> newsItems = await _newsService.CallAllApis(supplierName, country, language, pageNumber);
            return Ok(newsItems);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while fetching supplier news.");

            var response = new
            {
                error = ex.Message
            };
            return BadRequest(response);

        }
    }
}