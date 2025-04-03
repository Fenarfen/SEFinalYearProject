using CustardRM.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CustardRM.Backend.Controllers;

public class InventoryController : Controller
{
    private readonly IDatabaseService _databaseService;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;

    public InventoryController(IDatabaseService databaseService, IPasswordHasher passwordHasher, IJwtTokenService jwtTokenService)
    {
        _databaseService = databaseService;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
    }

    [HttpGet("api/inventory/no-filter")]
    public IActionResult GetInventoryNoFilter()
    {
        try
        {
            var result = _databaseService.GetStockItems();

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while processing your request. Please try again later.\n" + ex.ToString() });
        }
    }
}
