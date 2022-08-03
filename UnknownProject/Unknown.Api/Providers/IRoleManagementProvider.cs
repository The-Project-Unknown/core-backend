namespace Api.Providers;

public interface IRoleManagementProvider<TRole> 
{
    TRole GetRoleById(long id);
    List<TRole> GetAllRoles();
    List<TRole> GetUserRolesById(Guid id);
    List<TRole> GetUserRolesByGuid(Guid id);
}