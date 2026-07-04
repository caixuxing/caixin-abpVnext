using CaiXin.EntityFrameworkCore;
using CaiXin.NiuMa.Domain.Ports;
using CaiXin.NiuMa.Infrastructure.Dto;
using Dapper;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories.Dapper;

namespace CaiXin.NiuMa.Infrastructure.Repository
{
    internal sealed class MemberPageQuery(DapperRepository<CaiXinContext> _dbContextProvider)
        : IPageQuery<MemberPageQry, MemberPageDto>, ITransientDependency
    {
        public async Task<(List<MemberPageDto> list, long total)> PageQueryAsync(MemberPageQry request, CancellationToken cancellationToken = default)
        {
            var dbConnection = await _dbContextProvider.GetDbConnectionAsync();

            var data = await dbConnection.QueryAsync<MemberPageDto>("SELECT * FROM Users WHERE UserName = @UserName",
                  new { UserName = request.UserName });

            return (data.ToList(), 10L);
        }
    }
}