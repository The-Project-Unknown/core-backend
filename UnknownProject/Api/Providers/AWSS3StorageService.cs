using System.Net;
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

    public async Task<string> UploadFile(string bucketName, IFormFile formFile)
    {
        return await UploadFile(bucketName, "", formFile);
    }

    public async Task<string> UploadFile(string bucketName, string filePath, IFormFile formFile)
    {
        var location = $"{filePath}/{formFile.FileName}";
        
        using (var stream = formFile.OpenReadStream())
        {
            var putRequest = new PutObjectRequest
            {
                Key = location,
                BucketName = "uknown-api-bucket",
                InputStream = stream,
                AutoCloseStream = true,
                ContentType = formFile.ContentType
            };
            return location;
        }
    }

    public async Task<ListBucketsResponse> GetAllBuckets()
    {
        return await _s3BucketClient.ListBucketsAsync();
    }

    public async Task<ListObjectsResponse> GetFileNamesInBucket(string bucketName)
    {
        return await _s3BucketClient.ListObjectsAsync(bucketName, $"");
    }

    public async Task<PutBucketResponse> CreateBucket(string bucketName)
    {
        return await _s3BucketClient.PutBucketAsync(bucketName);
    }

    
    /*
    public async Task<byte[]> DownloadFileAsync(string file)
    {
        MemoryStream ms;

        try
        {
            GetObjectRequest getObjectRequest = new GetObjectRequest
            {
                BucketName = "uknown-api-bucket",
                Key = file
            };

            using (var response = await _s3BucketClient.GetObjectAsync(getObjectRequest))
            {
                if (response.HttpStatusCode == HttpStatusCode.OK)
                {
                    using (ms = new MemoryStream())
                    {
                        await response.ResponseStream.CopyToAsync(ms);
                    }
                }
            }

            if (ms is null || ms.ToArray().Length < 1)
                throw new FileNotFoundException(string.Format("The document '{0}' is not found", file));

            return ms.ToArray();
        }
        catch (Exception)
        {
            throw;
        }
    }
    */
}

public interface IAWSS3StorageService
{
    public Task<string> UploadFile(string bucketName, IFormFile formFile);
    
    public Task<string> UploadFile(string bucketName, string filePath, IFormFile formFile);

    public Task<ListBucketsResponse> GetAllBuckets();
    public Task<ListObjectsResponse> GetFileNamesInBucket(string bucketName);

    public Task<PutBucketResponse> CreateBucket(string bucketName);
    
}
