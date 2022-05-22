using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Api.Tests;

[Collection(nameof(APITestingCollection))]
public class Test2
{
    private readonly ApiIntegrationTestContext _apiIntegrationTestContext;

    public Test2(ApiIntegrationTestContext apiIntegrationTestContext)
    {
        _apiIntegrationTestContext = apiIntegrationTestContext;
    }
    [Fact]
    public async Task StatusTest()
    {
        var client = _apiIntegrationTestContext.Client;

        var res = await client.GetAsync("/status");

        res.EnsureSuccessStatusCode();
    }
    
    [Fact]
    public async Task TestStatusWithAuthorization()
    {
        var client = _apiIntegrationTestContext.Client;

        var res = await client.GetAsync("/status");

        res.EnsureSuccessStatusCode();
    }
}