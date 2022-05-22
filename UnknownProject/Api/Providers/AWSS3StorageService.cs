using Amazon.S3;
using Amazon.S3.Model;

namespace Api.Providers;



public class AWSS3StorageService : IAWSS3StorageService
{
    private readonly IAmazonS3 _s3BucketClient;
    
    public AWSS3StorageService(IAmazonS3 amazonS3Bucket)
    {
        _s3BucketClient = amazonS3Bucket;
    }
    
    public async Task<string> UploadFile(IFormFile formFile)
    {
        var location = $"uploads/{formFile.FileName}";
        
        using (var stream = formFile.OpenReadStream())
        {
            var putRequest = new PutObjectRequest
            {
                Key = location,
                BucketName = "upload-test-berv",
                InputStream = stream,
                AutoCloseStream = true,
                ContentType = formFile.ContentType
            };
            var response = await _s3BucketClient.PutObjectAsync(putRequest);
            return location;
        }
    }
    
}

public interface IAWSS3StorageService
{
    public Task<string> UploadFile(IFormFile formFile);
}
