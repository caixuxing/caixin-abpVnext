namespace CaiXin.NiuMa.Application.Contracts.Permissions
{
    public static class CorePermissions
    {
        public const string GroupName = "CaiXin.NiuMa";

        public static class Employees
        {
            public const string Default = GroupName + ".Employee";
            public const string Management = Default + ".Management";
            public const string Delete = Default + ".Delete";
            public const string Update = Default + ".Update";
            public const string Create = Default + ".Create";
            public const string ClearCache = Default + ".ClearCache";
        }
    }
}
