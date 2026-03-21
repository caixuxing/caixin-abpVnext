using Volo.Abp.Domain.Entities;

namespace CaiXin.NiuMa.Domain.Member;

public class User : Entity<Guid>
{
    public string Name { get; set; } = default!;

    public string Password { get; set; } = default!;

    public string Salt { get; set; } = default!;

    /// <summary>
    /// 创建用户
    /// </summary>
    /// <param name="name"></param>
    /// <param name="pwd"></param>
    /// <param name="salt"></param>
    /// <returns></returns>
    public User Cteate(Guid guid, string name, string pwd, string salt)
    {
        this.Id = guid;
        this.Name = name;
        this.Password = pwd;
        this.Salt = salt;
        return this;
    }
}