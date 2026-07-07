using CaiXin.NiuMa.Domain.Employees.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaiXin.EntityFrameworkCore.EntityConfigs
{
    public class SysUserConfig : IEntityTypeConfiguration<SysUser>
    {
        public void Configure(EntityTypeBuilder<SysUser> builder)
        {
            builder.ToTable("SysUser", t => t.HasComment("系统用户表"));
            builder.HasKey(p => p.Id);
            builder.Property(t => t.Id).HasColumnName("ID").HasMaxLength(50).ValueGeneratedNever().HasComment("主键ID");


        }
    }
}
