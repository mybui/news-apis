using Microsoft.AspNetCore.Mvc;
using CsvHelper;
using System.Globalization;
using CsvHelper.Configuration;

namespace news_apis.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SupplierGroupController : ControllerBase
{
    private readonly ILogger<SupplierGroupController> _logger;
    private readonly SupplierService _supplierService;

    public SupplierGroupController(ILogger<SupplierGroupController> logger)
    {
        _logger = logger;
        _supplierService = new SupplierService();
    }

    [HttpGet]
    public IEnumerable<Supplier> Get()
    {
        return _supplierService.GetSuppliers();
    }

}