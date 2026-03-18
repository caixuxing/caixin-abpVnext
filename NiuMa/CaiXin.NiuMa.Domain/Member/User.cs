using Volo.Abp.Domain.Entities;

namespace CaiXin.NiuMa.Domain.Member;

public class User : IEntity<Guid>
{
    public int Id { get; set; }

    Guid IEntity<Guid>.Id => throw new NotImplementedException();

    public object?[] GetKeys()
    {
        throw new NotImplementedException();
    }

    public static User Cteate()
    {
        return new User();
    }
}