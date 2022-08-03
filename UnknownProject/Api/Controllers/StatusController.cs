using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Unknown.DataAccess;

namespace Api.Controllers;

[ApiController, Route("/status")]
[Route("/ping")]
public class StatusController : ControllerBase
{
    private readonly ApiDbContext _db;
    private readonly ILogger<StatusController> _logger;

    public StatusController(ApiDbContext db, ILogger<StatusController> logger)
    {
        _db = db;
        _logger = logger;
    }
    
    
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Get()
    {
        return Ok("hello there");
    }
    
    [HttpGet("/test")]
    public IActionResult Get1()
    {
        return Ok();
    }
    
   /* 
    [AllowAnonymous]
    [HttpGet("/test589")]
    public IActionResult Get2()
    {
        List<TestingClassForDbContext> list = new();

        for (int i = 0; i < 10_000; i++)
        {
            list.Add(new TestingClassForDbContext
            {
                Id = i,
                Key = i.ToString(),
                Value = (i*2).ToString()
            });
        }

        Stopwatch stopwatch = new Stopwatch();
        _logger.LogInformation("Starting _db.SaveChanges()");
        stopwatch.Start();
        _db.TestingClassForDbContext.AddRange(list);
        _db.SaveChanges();
        _logger.LogInformation("Done _db.SaveChanges() {0}", stopwatch.Elapsed);
        stopwatch.Stop();
        
        _db.TestingClassForDbContext.RemoveRange(list);
        _db.SaveChanges();
        
        var t = _db.TestingClassForDbContext.ToList();
        return Ok(t);
    }
    */
}