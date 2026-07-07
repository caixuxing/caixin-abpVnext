using CaiXin.NiuMa.Domain.Employees.Entity;
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

        public virtual SysUser SysUser { get; private set; } = null!;


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






        public static Employee Create(
            Guid id,
            string employeeNumber,
            string fullName,
            string? email,
            string? phoneNumber,
            DateTime hireDate)
        {
            // 验证员工信息
            if (string.IsNullOrWhiteSpace(employeeNumber))
                throw new ArgumentException("工号不能为空", nameof(employeeNumber));
            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("姓名不能为空", nameof(fullName));


            var (Password, Salt) = UserPassword.Create("123456");
            // 创建员工
            var employee = new Employee
            {
                Id = id,
                EmployeeNumber = employeeNumber,
                FullName = fullName,
                Email = email,
                PhoneNumber = phoneNumber,
                HireDate = hireDate,
                Status = 1,
                SysUser = Entity.SysUser.Create(id, "ccx", Password, Salt)
            };

            // 添加本地事件
            employee.AddLocalEvent(new
            {



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
            //禁用账号
            SysUser?.Deactivate();

            // 发布离职事件->告知订阅者处理相关事项、跨聚合 同一进程内可使用本地事件总线。
            //AddLocalEvent(new EmployeeResignedEvent(Id, resignationDate));

            //分布式事件总线。
            // AddDistributedEvent(new EmployeeResignedEvent(Id, resignationDate));
        }


    }
}
