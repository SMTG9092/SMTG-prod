using Microsoft.AspNetCore.Mvc;
using SMTG.API.Models;
using SMTG.API.Services;

namespace SMTG.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SupabaseController : ControllerBase
{
    private readonly SupabaseService _supabase;

    public SupabaseController(SupabaseService supabase)
    {
        _supabase = supabase;
    }

    [HttpGet("test")]
    public async Task<IActionResult> Test()
    {
        try
        {
            var result = await _supabase.Client
                .From<Role>()
                .Get();

            return Ok(new
            {
                success = true,
                total = result.Models.Count
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                success = false,
                error = ex.Message
            });
        }
    }
}