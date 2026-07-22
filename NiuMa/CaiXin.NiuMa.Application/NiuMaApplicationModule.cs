using CaiXin.EventBus;
using CaiXin.NiuMa.Application.Authorization;
using CaiXin.NiuMa.Application.Contracts.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Authorization.Permissions;

namespace CaiXin.NiuMa.Application;

[DependsOn(
    typeof(NiuMaInfrastructureModule),
    typeof(NiuMaApplicationContractsModule),
    typeof(NiuMaDomainModule),
    typeof(CaiXinEventBusModule)
    )]
public class NiuMaApplicationModule : AbpModule
{

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // 注册授权处理器
        context.Services.AddSingleton<IAuthorizationHandler, UnifiedPermissionAuthorizationHandler>();

        // 注册资源授权检查器
        // context.Services.AddScoped<IResourceAuthorizationChecker, EmployeeResourceAuthorizationChecker>();

        // 配置授权策略
        Configure<AuthorizationOptions>(options =>
        {
            // 基础权限策略
            options.AddPolicy("EmployeeManagement", policy =>
                policy.RequireClaim("Permission",
                    CorePermissions.Employees.Management,
                    CorePermissions.Employees.Default));

            // 管理员策略
            options.AddPolicy("AdminOnly", policy =>
                policy.RequireRole("Admin"));

            // 自定义策略
            options.AddPolicy("EmployeeUpdate", policy =>
                policy.RequireAssertion(async context =>
                {
                    var permissionChecker = context.Resource as IPermissionChecker;
                    if (permissionChecker != null)
                    {
                        return await permissionChecker.IsGrantedAsync(
                            CorePermissions.Employees.Update);
                    }
                    return false;
                }));


            // 组合策略：需要同时拥有多个权限
            options.AddPolicy("EmployeeFullAccess", policy =>
                policy.RequireClaim("Permission",
                    CorePermissions.Employees.Default,
                    CorePermissions.Employees.Management,
                    CorePermissions.Employees.Create,
                    CorePermissions.Employees.Update,
                    CorePermissions.Employees.Delete));



        });

        // 配置权限值提供者（如果需要）
        //context.Services.AddSingleton<IPermissionValueProvider, PermissionValueProvider>();
    }
}