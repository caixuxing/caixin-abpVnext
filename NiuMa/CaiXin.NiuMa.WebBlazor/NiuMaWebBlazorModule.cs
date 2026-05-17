using BootstrapBlazor.Components;
using CaiXin.EntityFrameworkCore;
using CaiXin.NiuMa.Application;
using CaiXin.NiuMa.WebBlazor.Components;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.AspNetCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace CaiXin.NiuMa.WebBlazor
{
    [DependsOn(
           typeof(AbpAspNetCoreModule),
       typeof(AbpAutofacModule),
       typeof(NiuMaApplicationModule),
       typeof(CaiXinEntityFrameworkCoreModule)
   )]
    public class NiuMaWebBlazorModule : AbpModule
    {
        #region 中间件注入

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddMemoryCache();
            context.Services.AddHttpClient();
            ConfigureBlazor(context);
            context.Services.AddControllers();
        }

        /// <summary>
        /// Blazor
        /// </summary>
        /// <param name="context"></param>
        private void ConfigureBlazor(ServiceConfigurationContext context)
        {
            context.Services.AddRazorComponents()
               .AddInteractiveServerComponents();

            //context.Services.AddRazorPages();
            context.Services.AddServerSideBlazor();
            context.Services.AddBootstrapBlazor(options =>
            {
                // 统一设置 Toast 组件自动消失时间
                options.ToastDelay = 4000;
                options.ToastPlacement = Placement.TopEnd;
                options.SupportedCultures = new List<string> { "zh" };

                options.FallbackCulture = "zh";
                options.DefaultCultureInfo = "zh";
            });
            // 增加多语言支持配置信息
            context.Services.AddRequestLocalization<IOptionsMonitor<BootstrapBlazorOptions>>((localizerOption, blazorOption) =>
            {
                blazorOption.OnChange(op => Invoke(op));
                Invoke(blazorOption.CurrentValue);

                void Invoke(BootstrapBlazorOptions option)
                {
                    var supportedCultures = option.GetSupportedCultures();
                    localizerOption.SupportedCultures = supportedCultures;
                    localizerOption.SupportedUICultures = supportedCultures;
                }
            });
        }

        #endregion 中间件注入

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();
            var option = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            if (option != null)
            {
                app.UseRequestLocalization(option.Value);
            }
            app.UseStaticFiles();
            app.UseRouting();
            app.UseExceptionHandler("/Error");

            app.UseAntiforgery();
        }
    }
}