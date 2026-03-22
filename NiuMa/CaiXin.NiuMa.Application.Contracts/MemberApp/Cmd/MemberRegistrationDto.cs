namespace CaiXin.NiuMa.Application.Contracts.MemberApp.Commands
{
    public class MemberRegistrationDto
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; } = default!;
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; } = default!;

        /// <summary>
        /// 盐值
        /// </summary>
        public string Salt { get; set; } = default!;
    }
}