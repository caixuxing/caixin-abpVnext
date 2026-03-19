using CaiXin.EntityFrameworkCore;
using CaiXin.NiuMa.Domain.Member;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace CaiXin.NiuMa.Infrastructure.Repository
{
    public class UserRepository : EfCoreRepository<CaiXinContext, User, Guid>, IUserRepository, ITransientDependency
    {
        public UserRepository(IDbContextProvider<CaiXinContext> dbContextProvider) : base(dbContextProvider)
        {
        }


        public async Task<User?> FindByEmailAsync(string email)
        {

            var data = await (await GetDbSetAsync()).FirstOrDefaultAsync(u => u.Name == email);
            return data;
        }

        public async Task UpdateUserNamesBatchAsync(string oldName, string newName)
        {

            var dbContext = await GetDbContextAsync();
            var users = await dbContext.Set<User>()
                .Where(u => u.Name == oldName)
                .ToListAsync();
            foreach (var user in users)
            {
                user.Name = newName;
            }
            await dbContext.SaveChangesAsync();
        }
    }
}
