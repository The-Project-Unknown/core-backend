using Auth0.ManagementApi.Models;

namespace Api.Providers;

//Todo 
public class Auth0RoleManagementProvider : IRoleManagementProvider<Role>
{
    
    public Role GetRoleById(long id)
    {
        throw new NotImplementedException();
    }

    public List<Role> GetAllRoles()
    {
        throw new NotImplementedException();
    }

    public List<Role> GetUserRolesById(Guid id)
    {
        throw new NotImplementedException();
    }

    public List<Role> GetUserRolesByGuid(Guid id)
    {
        throw new NotImplementedException();
    }
}