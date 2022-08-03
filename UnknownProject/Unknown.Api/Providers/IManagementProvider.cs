namespace Api.Providers;

public interface IManagementProvider<TUser, TRole>
{
    public IUserManagementProvider<TUser> UserManagementProvider { get; set; }
    public IRoleManagementProvider<TRole> RoleManagementProvider { get; set; }
}