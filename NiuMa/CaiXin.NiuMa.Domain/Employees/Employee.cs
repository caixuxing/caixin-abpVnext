using CaiXin.NiuMa.Domain.Employees.Entity;
using CaiXin.NiuMa.Domain.Employees.EventDto;
using CaiXin.NiuMa.Domain.Member.ValueObjects;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace CaiXin.NiuMa.Domain.Employees
{
    /// <summary>
    /// 员工
    /// </summary>
    public class Employee : AggregateRoot<Guid>, IFullAuditedObject<string>, IMultiTenant
    {
        /// <summary>
        /// 员工编号
        /// </summary>
        public string EmployeeNumber { get; init; } = null!;

        /// <summary>
        /// 员工姓名
        /// </summary>
        public string FullName { get; private set; } = null!;

        /// <summary>
        /// 昵称
        /// </summary>
        public string? Nickname { get; private set; }

        /// <summary>
        /// 员工邮箱
        /// </summary>
        public string? Email { get; private set; }

        /// <summary>
        /// 员工手机号
        /// </summary>
        public string? PhoneNumber { get; private set; }

        /// <summary>
        /// 入职日期
        /// </summary>
        public DateTime HireDate { get; private set; }

        /// <summary>
        /// 员工状态[  1=>在职; 2=>已离职;3=>试用期; 4 => 请假中]
        /// </summary>
        public int Status { get; private set; }

        /// <summary>
        /// 系统用户
        /// </summary>
        public virtual SysUser? SysUser { get; private set; }

        public Guid? TenantId { get; init; }

        public Guid? CreatorId { get; init; }

        public string? Creator { get; init; }

        public DateTime CreationTime { get; init; }

        public Guid? LastModifierId { get; set; }

        public string? LastModifier { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public Guid? DeleterId { get; set; }

        public string? Deleter { get; set; }

        public DateTime? DeletionTime { get; set; }

        public bool IsDeleted { get; set; }

        /// <summary>
        /// 创建员工
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <param name="employeeNumber">员工工号</param>
        /// <param name="fullName">员工姓名</param>
        /// <param name="email">邮箱</param>
        /// <param name="phoneNumber">手机</param>
        /// <param name="hireDate">入职日期</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static Employee Create(Guid id, string employeeNumber, string fullName, string? email, string? phoneNumber, DateTime hireDate)
        {
            if (string.IsNullOrWhiteSpace(employeeNumber))
                throw new ArgumentException("工号不能为空", nameof(employeeNumber));
            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("姓名不能为空", nameof(fullName));

            var (Password, Salt) = UserPassword.Create("123456");

            var employee = new Employee
            {
                Id = id,
                EmployeeNumber = employeeNumber,
                FullName = fullName,
                Email = email,
                PhoneNumber = phoneNumber,
                HireDate = hireDate,
                Status = 1,
                SysUser = SysUser.Create(id, "ccx", Password, Salt)
            };
            employee.AddLocalEvent(new CreateEmployeeEto
            {
                Id = employee.Id,
                EmployeeNumber = employeeNumber,
                FullName = fullName,
            });

            return employee;
        }

        /// <summary>
        /// 离职
        /// </summary>
        public void Resign(DateTime resignationDate)
        {
            if (Status == 2) throw new InvalidOperationException("员工已经离职");
            Status = 2;
            SysUser?.Deactivate();
        }
    }
}