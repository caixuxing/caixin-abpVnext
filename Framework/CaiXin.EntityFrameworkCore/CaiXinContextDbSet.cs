using CaiXin.NiuMa.Domain.Employees;
using CaiXin.NiuMa.Domain.Employees.Entity;
using CaiXin.NiuMa.Domain.Member;
using Microsoft.EntityFrameworkCore;

namespace CaiXin.EntityFrameworkCore
{
    public partial class CaiXinContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<SysUser> SysUsers { get; set; }

        public DbSet<Employee> Employees { get; set; }
    }
}