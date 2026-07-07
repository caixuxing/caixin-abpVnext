using CaiXin.NiuMa.Domain.Member;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaiXin.EntityFrameworkCore.EntityConfigs
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User", t => t.HasComment("用户信息表"));
            builder.HasKey(p => p.Id);
            builder.Property(t => t.Id).HasColumnName("ID").HasMaxLength(50).ValueGeneratedNever().HasComment("主键ID");
            builder.OwnsOne(t => t.Name, c =>
            {
                c.Property(p => p.Value)
                    .HasColumnName("Name")
                    .HasMaxLength(50)
                    .HasComment("用户名称");

            });
            builder.OwnsOne(t => t.Password, pw =>
            {
                pw.Property(p => p.Password)
                    .HasColumnName("Password")
                    .HasMaxLength(50)
                    .HasComment("用户密码");

                pw.Property(p => p.Salt)
                    .HasColumnName("Salt")
                    .HasMaxLength(50)
                    .HasComment("盐值");
            });

            builder.Property(t => t.Creator).HasColumnName("Creator").HasMaxLength(20).HasComment("创建者");

            builder.Property(t => t.LastModifier).HasColumnName("LastModifier").HasMaxLength(20).HasComment("更新者");

            builder.Property(t => t.Deleter).HasColumnName("Deleter").HasMaxLength(20).HasComment("删除者");

        }
    }
}
