namespace CaiXin.NiuMa.Application.Contracts.MemberApp.Cmd
{
    public class MemberRegistrationDto
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; } = null!;
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; } = null!;

        /// <summary>
        /// 盐值
        /// </summary>
        public string Salt { get; set; } = null!;
    }
}