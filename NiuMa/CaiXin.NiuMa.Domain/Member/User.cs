using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace CaiXin.NiuMa.Domain.Member;

public class User : Entity<Guid>
{
    public string Name { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Salt { get; set; } = null!;

    protected User()
    { }

    private User(Guid id, string name, string password, string salt)
    {
        Name = name;
        Password = password;
        Salt = salt;
        Id = id;
    }

    /// <summary>
    /// 创建用户
    /// </summary>
    /// <param name="guId"></param>
    /// <param name="name"></param>
    /// <param name="pwd"></param>
    /// <param name="salt"></param>
    /// <returns></returns>
    public static User Create(Guid id, string name, string pwd, string salt)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("用户名不能为空");

        if (name.Length < 2 || name.Length > 20)
            throw new ArgumentException("用户名长度必须在2-20之间");

        return new User(id, name, pwd, salt);
    }
}