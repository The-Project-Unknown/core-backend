using Auth0.ManagementApi.Models;

namespace Api.Providers;

public class Auth0UserManagementProvider : IUserManagementProvider<User>
{
    public User GetUserById(long id)
    {
        throw new NotImplementedException();
    }

    public User GetUserByGuid(Guid id)
    {
        throw new NotImplementedException();
    }

    public User GetUserByEmail(Guid id)
    {
        throw new NotImplementedException();
    }

    public User UpdateUser(Guid id)
    {
        throw new NotImplementedException();
    }
}