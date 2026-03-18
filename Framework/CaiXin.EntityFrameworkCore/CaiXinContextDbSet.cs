using CaiXin.NiuMa.Domain.Member;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaiXin.EntityFrameworkCore
{
    public partial class CaiXinContext
    {
        public DbSet<User> Users { get; set; }
    }
}