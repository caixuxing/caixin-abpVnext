using CaiXin.NiuMa.Domain.Member;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace CaiXin.EntityFrameworkCore
{
    [ConnectionStringName("Default")]
    public partial class CaiXinContext : AbpDbContext<CaiXinContext>
    {
        public CaiXinContext(DbContextOptions<CaiXinContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}