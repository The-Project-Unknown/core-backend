using System.Net.Mime;
using Amazon.S3;
using Api.Providers;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
public class UploadController : ControllerBase
{
    private readonly IAWSS3StorageService _awsS3Service;
    private readonly IServiceProvider _serviceProvider;

    public UploadController(IAWSS3StorageService awsS3Service,IServiceProvider serviceProvider)
    {
        _awsS3Service = awsS3Service;
        _serviceProvider = serviceProvider;
    }

    [HttpPost("upload-to-s3")]
    public async Task<IActionResult> Upload(
        IFormFile file, 
        [FromQuery]string filePath,
        [FromQuery]string bucketName = "uknown-api-bucket"
        )
    {
        var path = await _awsS3Service.UploadFile(bucketName, filePath, file);
        
        return Ok(new { path });
    }
    
    [HttpGet("list-buckets")]
    public async Task<IActionResult> GetAllBuckets()
    {
        return Ok(await _awsS3Service.GetAllBuckets());
    }
    
    [HttpGet("list-bucket-content")]
    public async Task<IActionResult> GetBucketContent([FromQuery] string bucketName )
    {
        var response = await _awsS3Service.GetFileNamesInBucket(bucketName);

        var res = response.S3Objects.Select(x =>  x.Key).ToList();
        return Ok(res);
    }
    
    [HttpGet("list-bucket-content-detailed")]
    public async Task<IActionResult> GetBucketContentDetailed([FromQuery] string bucketName )
    {
        var response = await _awsS3Service.GetFileNamesInBucket(bucketName);

        var res = response.S3Objects.Select(x => new { x.Key, x.Size, x.LastModified }).ToList();
        return Ok(res);
    }
    
    [HttpGet("create-bucket")]
    public async Task<IActionResult> CreateBucket([FromQuery]string bucketName)
    {
        return Ok(await _awsS3Service.CreateBucket(bucketName));
    }

    [HttpGet("config")]
    public Task<IActionResult> GetAWSConfig()
    {
        var s3Client = _serviceProvider.GetRequiredService<IAmazonS3>();

        return Task.FromResult<IActionResult>(Ok(s3Client.Config));
    }
    
    [HttpGet("download-file")]
    public IActionResult DownloadFile([FromQuery]string filePath)
    {
        return PhysicalFile(filePath,"application/octet-stream" ,Path.GetFileName(filePath));
    }
    
    /*[HttpGet("down")]
    public IActionResult DownloadFile2([FromQuery]string filePath)
    {
        //var document = _awsS3Service. DownloadFileAsync(filePath).Result;

       // return File(document, "application/octet-stream", documentName);
    }
    */
}