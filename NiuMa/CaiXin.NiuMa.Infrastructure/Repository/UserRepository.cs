using CaiXin.EntityFrameworkCore;
using CaiXin.NiuMa.Domain.Member;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace CaiXin.NiuMa.Infrastructure.Repository
{
    public class UserRepository(IDbContextProvider<CaiXinContext> dbContextProvider)
        : EfCoreRepository<CaiXinContext, User, Guid>(dbContextProvider), IUserRepository, ITransientDependency
    {
        public async Task<User?> FindByEmailAsync(string email)
        {
            var data = await (await GetDbSetAsync())
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(u => u.Name.Value == email);
            return data;
        }

        public async Task UpdateUserNamesBatchAsync(string oldName, string newName)
        {
            var dbContext = await GetDbContextAsync();
            var users = await dbContext
                                .Set<User>()
                                .Where(u => u.Name.Value == oldName)
                                .ToListAsync();
            users.ForEach(item => item.ChangePassword(newName));
            await dbContext.SaveChangesAsync();
        }

        public async Task<bool> TestSql(User user, Func<string, string, string, Task<string>> factory)
        {
            var sql = await factory(user.Name.Value, user.Password.Password, user.Password.Salt);
            Console.Write(sql);
            return true;
        }
    }
}