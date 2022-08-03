using Microsoft.AspNetCore.Mvc;
using Unknown.DataAccess;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public abstract class AbstractFakeCrudController<T> : ControllerBase where T : BaseEntity
{
    private readonly IRepositoryManager _repositoryManager;

    protected AbstractFakeCrudController(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;
    }
    
    [HttpGet("get-all")]
    public IActionResult GetAll([FromQuery] int offset = 0, [FromQuery] int limit = Int32.MaxValue)
    {
        var t = _repositoryManager.Context.Set<T>().Skip(offset).Take(limit).ToList();
        return Ok(t);
    }

    [HttpGet("get")]
    public IActionResult Get([FromQuery] long id)
    {
        var t = _repositoryManager.Context.Set<T>().FirstOrDefault(x => x.Id == id);
        return Ok(t);
    }
    
    [HttpPost("create")]
    public IActionResult Create([FromBody] T entity)
    {
        _repositoryManager.Context.Set<T>().Add(entity);

        _repositoryManager.Context.SaveChanges();

        return Ok();
    }

    
    [HttpDelete("delete")]
    public IActionResult Delete([FromQuery] long id)
    {
        var tmp = _repositoryManager.Context.Set<T>().FirstOrDefault(x => x.Id == id);

        if (tmp == null)
            return BadRequest($"There is no {typeof(T)} with id: {id} !!!");
        
        _repositoryManager.Context.Set<T>().Remove(tmp);
        _repositoryManager.Context.SaveChanges();
        
        return Ok();
    }    
    
    [HttpPut("update")]
    public IActionResult Update([FromBody] T entity)
    {
        var tmp = _repositoryManager.Context.Set<T>().Find(entity);

        if (tmp == null)
            return BadRequest($"There is no {typeof(T)} with id: {entity.Id} !!!");
        
        _repositoryManager.Context.Set<T>().Update(tmp);
        _repositoryManager.Context.SaveChanges();
        
        return Ok();
    }
}