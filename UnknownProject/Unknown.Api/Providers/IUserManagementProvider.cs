namespace Api.Providers;

public interface IUserManagementProvider<TUser> 
{
    TUser GetUserById(long id);
    TUser GetUserByGuid(Guid id);
    TUser GetUserByEmail(Guid id);
    TUser UpdateUser(Guid id);
}