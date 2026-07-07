using CaiXin.NiuMa.Domain.Member;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaiXin.NiuMa.Infrastructure.EntityConfigs
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User", t => t.HasComment("用户信息表"));
            builder.HasKey(p => p.Id);
            builder.Property(t => t.Id).HasColumnName("ID").HasMaxLength(50).ValueGeneratedNever().HasComment("主键ID");
            builder.Property(t => t.Name).HasColumnName("Name").HasMaxLength(50).HasComment("用户名称");
            builder.Property(t => t.Password.Password).HasColumnName("Password").HasMaxLength(50).HasComment("用户密码");
            builder.Property(t => t.Password.Salt).HasColumnName("Salt").HasMaxLength(50).HasComment("盐值");
        }
    }
}
