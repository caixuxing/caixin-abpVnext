namespace CaiXin.NiuMa.Application.Contracts.EmployeeApp.Dto
{
    public record EmployeeDto
    {

        /// <summary>
        /// 员工编号
        /// </summary>
        public string EmployeeNumber { get; set; }

        /// <summary>
        /// 员工姓名
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string? Nickname { get; set; }

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

        /// <summary>
        /// 员工状态[  1=>在职; 2=>已离职;3=>试用期; 4 => 请假中]
        /// </summary>
        public int Status { get; set; }
    }
}
