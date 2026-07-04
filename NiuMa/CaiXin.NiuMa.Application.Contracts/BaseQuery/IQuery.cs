using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaiXin.NiuMa.Application.Contracts.BaseQuery
{
    public interface IQuery<TRqt, TDto>
    {
        public Task<TDto> QueryAsync(TRqt request, CancellationToken token = default);
    }
}