using CaiXin.EntityFrameworkCore;
using CaiXin.NiuMa.Application;
using CaiXin.NiuMa.Application.Contracts;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace CaiXin.NiuMa.WebBlazor
{
    [DependsOn(

       typeof(AbpAutofacModule),
       typeof(NiuMaApplicationModule),
       typeof(CaiXinEntityFrameworkCoreModule)
   )]
    public class NiuMaWebBlazorModule : AbpModule
    {
    }
}