using CaiXin.NiuMa.Domain.Employees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaiXin.EntityFrameworkCore.EntityConfigs
{

    public class EmployeeConfig : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable("Employee", t => t.HasComment("员工信息表"));
            builder.HasKey(p => p.Id);
            builder.Property(t => t.Id).HasColumnName("ID").HasMaxLength(50).ValueGeneratedNever().HasComment("主键ID");

            builder.Property(t => t.Creator).HasColumnName("Creator").HasMaxLength(20).HasComment("创建者");

            builder.Property(t => t.LastModifier).HasColumnName("LastModifier").HasMaxLength(20).HasComment("更新者");

            builder.Property(t => t.Deleter).HasColumnName("Deleter").HasMaxLength(20).HasComment("删除者");

        }
    }
}
