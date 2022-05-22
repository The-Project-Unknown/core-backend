using Api.Providers;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UploadController : ControllerBase
{
    private readonly IAWSS3StorageService _awsS3Service;

    public UploadController(IAWSS3StorageService awsS3Service)
    {
        _awsS3Service = awsS3Service;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload(/*[FromForm]*/ IFormFile file)
    {
        throw new NotImplementedException();
        
        var path = await _awsS3Service.UploadFile(file);
        
        return Ok(new { path });
    }
}