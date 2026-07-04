using CaiXin.NiuMa.Domain.Member.ValueObjects;
using Volo.Abp.Domain.Entities;

namespace CaiXin.NiuMa.Domain.Member;

public class User : Entity<Guid>
{
    public UserName Name { get; init; } = null!;

    public UserPassword Password { get; private set; } = null!;

    private User()
    { }

    private User(Guid id, UserName name, UserPassword password)
    {
        Name = name;
        Password = password;
        Id = id;
    }

    /// <summary>
    /// 创建用户
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    /// <param name="pwd"></param>
    /// <returns></returns>
    public static User Create(Guid id, string name, string pwd)
    {
        var userName = UserName.Create(name);
        var password = UserPassword.Create(pwd);
        return new User(id, userName, password);
    }

    /// <summary>
    /// 更改密码
    /// </summary>
    /// <param name="password"></param>
    /// <returns></returns>
    public User ChangePassword(string password)
    {
        Password = UserPassword.Create(password);
        return this;
    }
}