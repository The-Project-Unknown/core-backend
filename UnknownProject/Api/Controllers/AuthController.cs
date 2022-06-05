using System.ComponentModel.DataAnnotations;
using Auth0.ManagementApi;
using Auth0.ManagementApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly ManagementApiClient _managementApiClient;

    public AuthController(ILogger<AuthController> logger)
    {
        _logger = logger;
        _managementApiClient = new ManagementApiClient(
            "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6IklxUXcyQzJWZi15TG9kcWlSaWtKTyJ9.eyJpc3MiOiJodHRwczovL3Vua25vd24tcHJvamVjdC5ldS5hdXRoMC5jb20vIiwic3ViIjoiN3lOU3N3aUZYN1JUdWRkdkFwOWZsNDBqQ0txcWt5OWNAY2xpZW50cyIsImF1ZCI6Imh0dHBzOi8vdW5rbm93bi1wcm9qZWN0LmV1LmF1dGgwLmNvbS9hcGkvdjIvIiwiaWF0IjoxNjUzOTM1MjMxLCJleHAiOjE2NTQwMjE2MzEsImF6cCI6Ijd5TlNzd2lGWDdSVHVkZHZBcDlmbDQwakNLcXFreTljIiwic2NvcGUiOiJyZWFkOmNsaWVudF9ncmFudHMgY3JlYXRlOmNsaWVudF9ncmFudHMgZGVsZXRlOmNsaWVudF9ncmFudHMgdXBkYXRlOmNsaWVudF9ncmFudHMgcmVhZDp1c2VycyB1cGRhdGU6dXNlcnMgZGVsZXRlOnVzZXJzIGNyZWF0ZTp1c2VycyByZWFkOnVzZXJzX2FwcF9tZXRhZGF0YSB1cGRhdGU6dXNlcnNfYXBwX21ldGFkYXRhIGRlbGV0ZTp1c2Vyc19hcHBfbWV0YWRhdGEgY3JlYXRlOnVzZXJzX2FwcF9tZXRhZGF0YSByZWFkOnVzZXJfY3VzdG9tX2Jsb2NrcyBjcmVhdGU6dXNlcl9jdXN0b21fYmxvY2tzIGRlbGV0ZTp1c2VyX2N1c3RvbV9ibG9ja3MgY3JlYXRlOnVzZXJfdGlja2V0cyByZWFkOmNsaWVudHMgdXBkYXRlOmNsaWVudHMgZGVsZXRlOmNsaWVudHMgY3JlYXRlOmNsaWVudHMgcmVhZDpjbGllbnRfa2V5cyB1cGRhdGU6Y2xpZW50X2tleXMgZGVsZXRlOmNsaWVudF9rZXlzIGNyZWF0ZTpjbGllbnRfa2V5cyByZWFkOmNvbm5lY3Rpb25zIHVwZGF0ZTpjb25uZWN0aW9ucyBkZWxldGU6Y29ubmVjdGlvbnMgY3JlYXRlOmNvbm5lY3Rpb25zIHJlYWQ6cmVzb3VyY2Vfc2VydmVycyB1cGRhdGU6cmVzb3VyY2Vfc2VydmVycyBkZWxldGU6cmVzb3VyY2Vfc2VydmVycyBjcmVhdGU6cmVzb3VyY2Vfc2VydmVycyByZWFkOmRldmljZV9jcmVkZW50aWFscyB1cGRhdGU6ZGV2aWNlX2NyZWRlbnRpYWxzIGRlbGV0ZTpkZXZpY2VfY3JlZGVudGlhbHMgY3JlYXRlOmRldmljZV9jcmVkZW50aWFscyByZWFkOnJ1bGVzIHVwZGF0ZTpydWxlcyBkZWxldGU6cnVsZXMgY3JlYXRlOnJ1bGVzIHJlYWQ6cnVsZXNfY29uZmlncyB1cGRhdGU6cnVsZXNfY29uZmlncyBkZWxldGU6cnVsZXNfY29uZmlncyByZWFkOmhvb2tzIHVwZGF0ZTpob29rcyBkZWxldGU6aG9va3MgY3JlYXRlOmhvb2tzIHJlYWQ6YWN0aW9ucyB1cGRhdGU6YWN0aW9ucyBkZWxldGU6YWN0aW9ucyBjcmVhdGU6YWN0aW9ucyByZWFkOmVtYWlsX3Byb3ZpZGVyIHVwZGF0ZTplbWFpbF9wcm92aWRlciBkZWxldGU6ZW1haWxfcHJvdmlkZXIgY3JlYXRlOmVtYWlsX3Byb3ZpZGVyIGJsYWNrbGlzdDp0b2tlbnMgcmVhZDpzdGF0cyByZWFkOmluc2lnaHRzIHJlYWQ6dGVuYW50X3NldHRpbmdzIHVwZGF0ZTp0ZW5hbnRfc2V0dGluZ3MgcmVhZDpsb2dzIHJlYWQ6bG9nc191c2VycyByZWFkOnNoaWVsZHMgY3JlYXRlOnNoaWVsZHMgdXBkYXRlOnNoaWVsZHMgZGVsZXRlOnNoaWVsZHMgcmVhZDphbm9tYWx5X2Jsb2NrcyBkZWxldGU6YW5vbWFseV9ibG9ja3MgdXBkYXRlOnRyaWdnZXJzIHJlYWQ6dHJpZ2dlcnMgcmVhZDpncmFudHMgZGVsZXRlOmdyYW50cyByZWFkOmd1YXJkaWFuX2ZhY3RvcnMgdXBkYXRlOmd1YXJkaWFuX2ZhY3RvcnMgcmVhZDpndWFyZGlhbl9lbnJvbGxtZW50cyBkZWxldGU6Z3VhcmRpYW5fZW5yb2xsbWVudHMgY3JlYXRlOmd1YXJkaWFuX2Vucm9sbG1lbnRfdGlja2V0cyByZWFkOnVzZXJfaWRwX3Rva2VucyBjcmVhdGU6cGFzc3dvcmRzX2NoZWNraW5nX2pvYiBkZWxldGU6cGFzc3dvcmRzX2NoZWNraW5nX2pvYiByZWFkOmN1c3RvbV9kb21haW5zIGRlbGV0ZTpjdXN0b21fZG9tYWlucyBjcmVhdGU6Y3VzdG9tX2RvbWFpbnMgdXBkYXRlOmN1c3RvbV9kb21haW5zIHJlYWQ6ZW1haWxfdGVtcGxhdGVzIGNyZWF0ZTplbWFpbF90ZW1wbGF0ZXMgdXBkYXRlOmVtYWlsX3RlbXBsYXRlcyByZWFkOm1mYV9wb2xpY2llcyB1cGRhdGU6bWZhX3BvbGljaWVzIHJlYWQ6cm9sZXMgY3JlYXRlOnJvbGVzIGRlbGV0ZTpyb2xlcyB1cGRhdGU6cm9sZXMgcmVhZDpwcm9tcHRzIHVwZGF0ZTpwcm9tcHRzIHJlYWQ6YnJhbmRpbmcgdXBkYXRlOmJyYW5kaW5nIGRlbGV0ZTpicmFuZGluZyByZWFkOmxvZ19zdHJlYW1zIGNyZWF0ZTpsb2dfc3RyZWFtcyBkZWxldGU6bG9nX3N0cmVhbXMgdXBkYXRlOmxvZ19zdHJlYW1zIGNyZWF0ZTpzaWduaW5nX2tleXMgcmVhZDpzaWduaW5nX2tleXMgdXBkYXRlOnNpZ25pbmdfa2V5cyByZWFkOmxpbWl0cyB1cGRhdGU6bGltaXRzIGNyZWF0ZTpyb2xlX21lbWJlcnMgcmVhZDpyb2xlX21lbWJlcnMgZGVsZXRlOnJvbGVfbWVtYmVycyByZWFkOmVudGl0bGVtZW50cyByZWFkOmF0dGFja19wcm90ZWN0aW9uIHVwZGF0ZTphdHRhY2tfcHJvdGVjdGlvbiByZWFkOm9yZ2FuaXphdGlvbnNfc3VtbWFyeSByZWFkOm9yZ2FuaXphdGlvbnMgdXBkYXRlOm9yZ2FuaXphdGlvbnMgY3JlYXRlOm9yZ2FuaXphdGlvbnMgZGVsZXRlOm9yZ2FuaXphdGlvbnMgY3JlYXRlOm9yZ2FuaXphdGlvbl9tZW1iZXJzIHJlYWQ6b3JnYW5pemF0aW9uX21lbWJlcnMgZGVsZXRlOm9yZ2FuaXphdGlvbl9tZW1iZXJzIGNyZWF0ZTpvcmdhbml6YXRpb25fY29ubmVjdGlvbnMgcmVhZDpvcmdhbml6YXRpb25fY29ubmVjdGlvbnMgdXBkYXRlOm9yZ2FuaXphdGlvbl9jb25uZWN0aW9ucyBkZWxldGU6b3JnYW5pemF0aW9uX2Nvbm5lY3Rpb25zIGNyZWF0ZTpvcmdhbml6YXRpb25fbWVtYmVyX3JvbGVzIHJlYWQ6b3JnYW5pemF0aW9uX21lbWJlcl9yb2xlcyBkZWxldGU6b3JnYW5pemF0aW9uX21lbWJlcl9yb2xlcyBjcmVhdGU6b3JnYW5pemF0aW9uX2ludml0YXRpb25zIHJlYWQ6b3JnYW5pemF0aW9uX2ludml0YXRpb25zIGRlbGV0ZTpvcmdhbml6YXRpb25faW52aXRhdGlvbnMiLCJndHkiOiJjbGllbnQtY3JlZGVudGlhbHMifQ.rfMU3rL9u8H7MFd0-nzEGMySdBGILughB-bD72qXinjyA8tpuWXydSx1d3qNZM1NW2PRLRViq_jCDsQ-7VKyb47A_hqjh5mWotA--VgNIwGW769bM1QXzqhni3HlVLMWvMxHCrH80BjDFk-pB0r-3TVTCt2PlBK6xLwsinYa6fs7hf4wawD136jyWcmHTIG9Trrk4dj3fUfUnUWGRXk_P1-PA_V2WFBsCQSKUKKUzwg7CVzG1XNMhUOPkkWDukXlGYfSWMzPstQIT-_FsT6WOas5TS8VSujZNt2LSG6WD68dV_LF727dDQBIkoRJuDOUPxsl-3lzWy2WslvG2yc5_A", 
            "unknown-project.eu.auth0.com");
    }
    
    [HttpGet("public")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Public()
    {
        return Ok(new
        {
            Message = "Hello from a public endpoint! You don't need to be authenticated to see this."
        });
    }

    [HttpGet("private")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult Private()
    {
        return Ok(new
        {
            Message = "Hello from a private endpoint! You need to be authenticated to see this."
        });
    }

    [HttpGet("private-scoped")]
    [Authorize(nameof(AuthorizePolicy.read))]
    [Authorize(nameof(AuthorizePolicy.write))]
    public IActionResult Scoped()
    {
        return Ok(new
        {
            Message = "Hello from a private endpoint! You need to be authenticated and have a permissions of read to see this."
        });
    }


    [HttpGet("user")]
    public async Task<IActionResult> GetUser([FromQuery] string email)
    {
        var allClients = await _managementApiClient.Users.GetUsersByEmailAsync(email);

        if (allClients is not  null)
        {
            allClients.ToList().ForEach(x =>
            {
                Console.WriteLine(x.FullName);
                Console.WriteLine(x.Email);
                Console.WriteLine(x.NickName);
            });
        }
        else
        {
            _logger.LogError("fuck!! there is null pointer" );
        }

        return Ok(allClients);
    }
    
    
    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _managementApiClient.Users.GetAllAsync(new GetUsersRequest());

        if (users is not null)
        {
            return Ok(users.Select(x => new
                {
                    x.UserId, x.Email, x.Picture, x.PhoneNumber, x.EmailVerified
                })
                .ToList());
        }
        else
        {
            _logger.LogError("fuck!! there is null pointer" );
            return BadRequest("Something went wrong");
        }
    }
    
    [HttpPost("create-user")]
    public async Task<IActionResult> CreateAllUsers([FromBody, Required] UserDTO userDTo)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Check what you are sending you jackass !!");
        }
        
        var tmp = new UserCreateRequest
        {
            Email = @$"pepaZDepa+{new Random(DateTime.UtcNow.Millisecond).Next(1,100_000)}@gmail.com",
            Password = "aaaaaAAAAA12345***",
            Connection = "Username-Password-Authentication"
        };
        
        var user = await _managementApiClient.Users.CreateAsync(tmp);

        if (user is not  null)
        {
            _logger.LogInformation(user.UserId);
        }
        else
        {
            _logger.LogError("fuck!! there is null pointer" );
        }

        return Ok(user);
    }
    
    [HttpDelete("del-user")]
    public async Task<IActionResult> DeleteUser([FromQuery, Required] string userId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Check what you are sending you jackass !!");
        }
        
        
        try
        {
            var user = await _managementApiClient.Users.GetAsync(userId.Trim());

            await _managementApiClient.Users.DeleteAsync("auth0|629513efdab4b400688694b1");
            
            return Ok(user);
        }
        catch(System.Exception e)
        {
            _logger.LogError(e.Message);
            
            return BadRequest(e.Message);
        }
    }


}
public class UserDTO
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Username { get; set; } = null!;
}
