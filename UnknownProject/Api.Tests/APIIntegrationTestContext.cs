using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Api.Tests;

public class ApiIntegrationTestContext
{
    public WebApplicationFactory<Program> Factory { get; set; }
    public HttpClient Client { get; set; }
    
    public ApiIntegrationTestContext()
    {
        Factory = new WebApplicationFactory<Program>();
        Client = Factory.CreateClient();
    }
}
