using CaiXin.NiuMa.Domain.Employees.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaiXin.EntityFrameworkCore.EntityConfigs
{
    public class SysUserConfig : IEntityTypeConfiguration<SysUser>
    {
        public void Configure(EntityTypeBuilder<SysUser> builder)
        {
            builder.ToTable(nameof(SysUser), t => t.HasComment("系统用户表"));
            builder.HasKey(p => p.Id);
            builder.Property(t => t.Id).HasColumnName("ID").HasMaxLength(50).ValueGeneratedNever().HasComment("主键ID");
            builder.Property(t => t.UserName).HasColumnName("UserName").HasMaxLength(50).HasComment("用户名");
            builder.Property(t => t.PasswordHash).HasColumnName("PasswordHash").HasMaxLength(100).HasComment("密码");
            builder.Property(t => t.Salt).HasColumnName("Salt").HasMaxLength(100).HasComment("盐值");
            builder.Property(t => t.IsActive).HasColumnName("IsActive").HasComment("是否激活");
            builder.Property(t => t.LastLoginTime).HasColumnName("LastLoginTime").HasComment("最后登录时间");
            builder.Property(t => t.CreationTime).HasColumnName("CreationTime").HasComment("创建时间");
            builder.Property(s => s.EmployeeId).HasColumnName("EmployeeId").HasComment("所属员工ID");

            builder
                .HasOne(s => s.Employee)
                .WithOne(e => e.SysUser)
                .HasForeignKey<SysUser>(s => s.EmployeeId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull); //如果删除员工，用户表的EmployeeId 字段置空
        }
    }
}
