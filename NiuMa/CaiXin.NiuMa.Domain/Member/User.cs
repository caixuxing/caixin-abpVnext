using Volo.Abp.Domain.Entities;

namespace CaiXin.NiuMa.Domain.Member;

public class User : Entity<Guid>
{
    public string Name { get; set; } = default!;

    public string Password { get; set; } = default!;

    public string Salt { get; set; } = default!;

    public static User Cteate()
    {
        return new User();
    }
}