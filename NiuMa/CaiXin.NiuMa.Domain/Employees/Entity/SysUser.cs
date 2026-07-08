using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;

namespace CaiXin.NiuMa.Domain.Employees.Entity;

public class SysUser : Entity<Guid>, IHasCreationTime
{
    /// <summary>
    /// 所属员工ID
    /// </summary>
    public Guid? EmployeeId { get; init; }


    public virtual Employee? Employee { get; private set; }


    /// <summary>
    /// 登录用户名
    /// </summary>
    public string UserName { get; private set; } = null!;

    /// <summary>
    ///密码哈希
    /// </summary>
    public string PasswordHash { get; private set; } = null!;

    /// <summary>
    /// 密码盐
    /// </summary>
    public string Salt { get; private set; } = null!;

    /// <summary>
    /// 账号是否启用
    /// </summary>
    public bool IsActive { get; private set; } = false;

    /// <summary>
    /// 最后登录时间
    /// </summary>
    public DateTime? LastLoginTime { get; private set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreationTime { get; init; }

    private SysUser()
    { }

    public static SysUser Create(Guid employeeId, string userName, string passwordHash, string salt)
    {
        return new SysUser
        {
            EmployeeId = employeeId,
            UserName = userName,
            PasswordHash = passwordHash,
            Salt = salt,
            IsActive = true
        };
    }

    /// <summary>
    /// 启用账号
    /// </summary>
    internal void Activate()
    {
        IsActive = true;
    }

    /// <summary>
    /// 禁用账号
    /// </summary>
    internal void Deactivate()
    {
        IsActive = false;
    }

    /// <summary>
    /// 记录登录时间
    /// </summary>
    internal void RecordLogin()
    {
        LastLoginTime = DateTime.UtcNow;
    }
}