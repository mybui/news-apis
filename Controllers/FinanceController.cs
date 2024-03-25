using Microsoft.AspNetCore.Mvc;
namespace news_apis.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FinanceController : ControllerBase
{
    private readonly ILogger<FinanceController> _logger;
    private readonly IConfiguration Configuration;
    private readonly FinanceService _financeService;

    public FinanceController(ILogger<FinanceController> logger, IConfiguration configuration)
    {
        _logger = logger;
        Configuration = configuration;
        _financeService = new(Configuration);
    }

    [HttpGet]
    public async Task<IActionResult> Get(string supplierName, string currency = "USD")
    {
        try
        {
            FinanceInfo financials = await _financeService.CallAllApis(supplierName, currency);
            return Ok(financials);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while fetching financial data.");
            var response = new
            {
                error = ex.Message
            };
            return BadRequest(response);

        }
    }
}