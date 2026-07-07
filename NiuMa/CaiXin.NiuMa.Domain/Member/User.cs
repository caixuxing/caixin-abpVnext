using CaiXin.NiuMa.Domain.Member.ValueObjects;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace CaiXin.NiuMa.Domain.Member;

public class User : AggregateRoot<Guid>, IFullAuditedObject<string>, IMultiTenant
{
    public UserName Name { get; init; } = null!;

    public UserPassword Password { get; private set; } = null!;

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




    private User()
    { }


    private User(Guid id, UserName userName, UserPassword password)
        : base(id)
    {
        Name = userName;
        Password = password;
        // 添加事件
        AddLocalEvent(new { userName = userName });
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
        var user = new User(id, userName, password);
        user.AddLocalEvent(new { });
        return user;
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