using Api.Controllers;
using Auth0.ManagementApi.Models;

namespace Api.Providers;

// Just for educational purposes - why use generics ?
public class MyManagementProvider : IManagementProvider<UserDTO, Role>
{
    public IUserManagementProvider<UserDTO> UserManagementProvider { get; set; }
    public IRoleManagementProvider<Role> RoleManagementProvider { get; set; }

    public MyManagementProvider(
        IUserManagementProvider<UserDTO> userManagementProvider,
        IRoleManagementProvider<Role> roleManagementProvider)
    {
        UserManagementProvider = userManagementProvider;
        RoleManagementProvider = roleManagementProvider;
    }
}