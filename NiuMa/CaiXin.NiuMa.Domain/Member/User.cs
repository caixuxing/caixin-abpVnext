using Volo.Abp.Domain.Entities;

namespace CaiXin.NiuMa.Domain.Member;

public class User : Entity<Guid>
{
    public string Name { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Salt { get; set; } = null!;

    /// <summary>
    /// 创建用户
    /// </summary>
    /// <param name="guId"></param>
    /// <param name="name"></param>
    /// <param name="pwd"></param>
    /// <param name="salt"></param>
    /// <returns></returns>
    public User Create(Guid guId, string name, string pwd, string salt)
    {
        this.Id = guId;
        this.Name = name;
        this.Password = pwd;
        this.Salt = salt;

        return this;
    }
}