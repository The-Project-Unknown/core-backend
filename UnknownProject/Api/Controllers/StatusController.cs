using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController, Route("/status")]
[Route("/ping")]
public class StatusController : ControllerBase
{
    private readonly ApiDbContext _db;

    public StatusController(ApiDbContext db)
    {
        _db = db;
    }
    
    
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("hello there");
    }
    
    [HttpGet("/test")]
    public IActionResult Get1()
    {
        var t = _db.TestingClassForDbContext.ToList();
        return Ok(t);
    }
}