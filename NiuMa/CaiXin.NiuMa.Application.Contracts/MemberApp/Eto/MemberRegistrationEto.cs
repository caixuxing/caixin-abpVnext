using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaiXin.NiuMa.Application.Contracts.MemberApp.Eto
{
    public class MemberRegistrationEto
    {
        public int OrderId { get; set; }

        public int UserId { get; set; }

        public string UserPhone { get; set; }

        public decimal TotalAmount { get; set; }
    }
}