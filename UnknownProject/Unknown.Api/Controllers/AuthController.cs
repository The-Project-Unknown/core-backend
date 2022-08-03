using System.ComponentModel.DataAnnotations;
using Api.Providers;
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

    public AuthController(ILogger<AuthController> logger, IManagementConnection managementConnection,IConfiguration configuration)
    {
        _logger = logger;
        var _managementApiClient = new ManagementApiClient(configuration["Management:Token"], configuration["Management:Domain"], managementConnection);
    }


    [HttpGet("user")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
        catch(Exception e)
        {
            _logger.LogError(e.Message);
            
            return BadRequest(e.Message);
        }
    }


}
public class UserDTO
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string Username { get; set; }
}
