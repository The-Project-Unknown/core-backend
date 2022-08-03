using Microsoft.AspNetCore.Authorization;

namespace Api.Auth
{
    public class HasScopeHandler : AuthorizationHandler<HasPermissionRequirement>
    {
        private readonly ILogger<HasPermissionRequirement> _logger;

        public HasScopeHandler(ILogger<HasPermissionRequirement> logger)
        {
            _logger = logger;
        }
        
        
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HasPermissionRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == "permissions" && c.Issuer == requirement.Issuer))
                return Task.CompletedTask;

            var userClaims = context.User.FindAll(c => c.Type == "permissions" && c.Issuer == requirement.Issuer).ToList();

            if (userClaims.Any(s => s.Value == requirement.Permission))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}