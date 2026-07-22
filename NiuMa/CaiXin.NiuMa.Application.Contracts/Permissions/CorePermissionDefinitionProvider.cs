using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace CaiXin.NiuMa.Application.Contracts.Permissions
{
    public class CorePermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var bloggingGroup = context.AddGroup(CorePermissions.GroupName, L("Permission:Core"));
            var employees = bloggingGroup.AddPermission(CorePermissions.Employees.Default, L("Permission:Employee"));
            employees.AddChild(CorePermissions.Employees.Management, L("Permission:Management"));
            employees.AddChild(CorePermissions.Employees.Update, L("Permission:Edit"));
            employees.AddChild(CorePermissions.Employees.Delete, L("Permission:Delete"));
            employees.AddChild(CorePermissions.Employees.Create, L("Permission:Create"));
            employees.AddChild(CorePermissions.Employees.ClearCache, L("Permission:ClearCache"));

        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<CoreResource>(name);
        }
    }
    public class CoreResource
    {
    }


}
