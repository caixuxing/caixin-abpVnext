using CaiXin.NiuMa.Domain.Member;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace CaiXin.NiuMa.Domain.DataSeed
{
    public class NiuMaDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IRepository<User, Guid> _userRepository;

        private readonly IGuidGenerator _guidGenerator;

        public NiuMaDataSeedContributor(IRepository<User, Guid> userRepository, IGuidGenerator guidGenerator)

        {
            _userRepository = userRepository;
            _guidGenerator = guidGenerator;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            var userEntity = User.Create(_guidGenerator.Create(), "caixin", "11360847", "123456");
            await _userRepository.InsertAsync(userEntity);
        }
    }
}