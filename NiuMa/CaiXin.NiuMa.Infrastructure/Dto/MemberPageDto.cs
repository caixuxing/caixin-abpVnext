using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaiXin.NiuMa.Infrastructure.Dto
{
    public class MemberPageDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
    }

    public class MemberPageQry
    {
        public string UserName { get; set; }
    }
}