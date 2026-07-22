using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Volo.Abp.Authorization;
using Volo.Abp.Authorization.Permissions;

namespace CaiXin.NiuMa.Application.Authorization
{
    [ExposeServices(typeof(IAuthorizationHandler))]
    public class UnifiedPermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IPermissionChecker _permissionChecker;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UnifiedPermissionAuthorizationHandler(
            IPermissionChecker permissionChecker,
            IHttpContextAccessor httpContextAccessor)
        {
            _permissionChecker = permissionChecker;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            // 获取用户身份
            var user = context.User;
            if (!user.Identity.IsAuthenticated)
            {
                context.Fail();
                return;
            }

            // 获取需要检查的权限列表（支持多权限）
            var permissions = requirement.PermissionName.Split(',')
                .Select(p => p.Trim())
                .Where(p => !string.IsNullOrEmpty(p))
                .ToList();

            // 检查是否拥有任一权限
            foreach (var permission in permissions)
            {
                if (await _permissionChecker.IsGrantedAsync(permission))
                {
                    context.Succeed(requirement);
                    return;
                }
            }

            // 所有权限检查失败
            context.Fail();
        }
    }

    // 扩展权限需求，支持多个权限
    public class MultiPermissionRequirement : PermissionRequirement
    {
        public MultiPermissionRequirement(string permissionName)
            : base(permissionName)
        {
        }

        public string[] Permissions => PermissionName.Split(',').Select(p => p.Trim()).ToArray();
    }
}
