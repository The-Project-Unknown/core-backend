using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using Auth0.ManagementApi.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Api.Providers;

public class Auth0ManagementProvider : IManagementProvider<User, Role>
{
    private readonly ILogger<Auth0ManagementProvider> _logger;
    private readonly IConfiguration _configuration;
    //private readonly IOptions<Auth0Config> _options;
    public IUserManagementProvider<User> UserManagementProvider { get; set; }
    public IRoleManagementProvider<Role> RoleManagementProvider { get; set; }

    public JwtSecurityToken? AccessToken { get; set; } 

    public Auth0ManagementProvider(
        ILogger<Auth0ManagementProvider> logger,
        Auth0UserManagementProvider userManagementProvider,
        Auth0RoleManagementProvider roleManagementProvider,
        IConfiguration configuration
        //TODO P5IDAT IOPTIONS     
        //IOptions<Auth0Config> options
    )
    {
        _logger = logger;
        _configuration = configuration;
        UserManagementProvider = userManagementProvider;
        RoleManagementProvider = roleManagementProvider;
    }

    public async Task<JwtSecurityToken> GetAccessTokenFromAuth0()
    {
        var config = _configuration.GetSection("Auth0:Management").Get<Auth0ManagementConfig>();
        
        using var httpClient = new HttpClient();
        using var request = new HttpRequestMessage(new HttpMethod("POST"), "https://unknown-project.eu.auth0.com/oauth/token");
        
        var contentList = new List<string>();
        contentList.Add("grant_type=client_credentials");
        contentList.Add($"client_id={config.ClientId}");
        contentList.Add($"client_secret={config.ClientSecret}");
        contentList.Add($"audience={config.Audience}");
        request.Content = new StringContent(string.Join("&", contentList));
        request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded"); 

        var httpResponse = await httpClient.SendAsync(request);
       
        if (httpResponse.Content.Headers.ContentType?.MediaType == "application/json")
        {
            var contentStream = await httpResponse.Content.ReadAsStreamAsync();

            using var streamReader = new StreamReader(contentStream);
            using var jsonReader = new JsonTextReader(streamReader);

            JsonSerializer serializer = new JsonSerializer();

            try
            {
                var token =  serializer.Deserialize<Auth0ManagmentTokenResponce>(jsonReader);
                
                var handler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadJwtToken(token?.access_token);
                
                _logger.LogInformation("New Management Token received");
                
                return jwtSecurityToken;
            }
            catch(JsonReaderException)
            {
                _logger.LogError("Invalid JSON.");
            } 
        }
        else
        {
            _logger.LogError("HTTP Response was invalid and cannot be deserialised.");
        }

        throw new ApplicationException("FUCK YOU");
    } 
}