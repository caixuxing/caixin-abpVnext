using Volo.Abp.Domain.Repositories;

namespace CaiXin.NiuMa.Domain.Member
{
    public interface IUserRepository : IRepository<User, Guid>
    {
    }
}
