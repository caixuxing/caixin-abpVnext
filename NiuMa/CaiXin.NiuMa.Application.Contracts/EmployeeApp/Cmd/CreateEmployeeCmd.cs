namespace CaiXin.NiuMa.Application.Contracts.EmployeeApp.Cmd
{
    public class CreateEmployeeCmd
    {
        /// <summary>
        /// 员工姓名
        /// </summary>
        public string FullName { get; set; } = null!;
        /// <summary>
        /// 员工邮箱
        /// </summary>
        public string? Email { get; set; }
        /// <summary>
        /// 员工手机号
        /// </summary>
        public string? PhoneNumber { get; set; }
        /// <summary>
        /// 入职日期
        /// </summary>
        public DateTime HireDate { get; set; }

    }

}
