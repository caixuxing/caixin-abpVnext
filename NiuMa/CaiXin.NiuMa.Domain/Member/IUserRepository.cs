using Volo.Abp.Domain.Repositories;

namespace CaiXin.NiuMa.Domain.Member
{
    public interface IUserRepository : IRepository<User, Guid>
    {
        public Task<bool> TestSql(User user, Func<string, string, string, Task<string>> factory);
    }
}