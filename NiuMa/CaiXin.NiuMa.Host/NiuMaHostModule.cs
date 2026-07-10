using CaiXin.NiuMa.Application;
using CaiXin.NiuMa.Application.Contracts;
using CaiXin.NiuMa.Host.Filter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.OpenApi.Models;
using NUglify.Helpers;
using Swashbuckle.AspNetCore.SwaggerUI;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
using Volo.Abp.Swashbuckle;

namespace CaiXin.NiuMa.Host
{

    [DependsOn(
             typeof(AbpAspNetCoreMvcModule),
             typeof(AbpAutofacModule),
             typeof(NiuMaApplicationContractsModule),
             typeof(NiuMaApplicationModule),
             typeof(AbpSwashbuckleModule)
    )]
    public class NiuMaHostModule : AbpModule
    {

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddControllers(opt =>
            {
                //opt.ModelValidatorProviders.Clear();
                opt.Filters.Add(new ServiceFilterAttribute(typeof(ResultWrapperFilter)));
                opt.Filters.Add(new ServiceFilterAttribute(typeof(ResultExceptionFilter)));
                //opt.EnableEndpointRouting = false;
            })
             .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
             .AddApplicationPart(typeof(NiuMaApplicationModule).Assembly)
             .AddControllersAsServices();


            context.Services.AddAbpSwaggerGen(
                      options =>
                      {
                          options.SwaggerDoc("v1", new OpenApiInfo
                          {
                              Title = "NiuMa API",
                              Version = "v1"
                          });
                          Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "CaiXin.NiuMa.*.xml")
                          .Select(x => x)
                          .ForEach(item => options.IncludeXmlComments(item, true));

                          options.DocInclusionPredicate((docName, description) => true);
                      }
                  );




            base.ConfigureServices(context);
        }







        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<AbpAspNetCoreMvcOptions>(options =>
            {
                options.ConventionalControllers.Create(typeof(NiuMaApplicationModule).Assembly);
            });
            base.PreConfigureServices(context);
        }



        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "YueEbk API");
                options.RoutePrefix = "doc";
                options.DefaultModelsExpandDepth(-1);
                options.DocExpansion(DocExpansion.None);
            });

            app.UseConfiguredEndpoints();
            base.OnApplicationInitialization(context);
        }


    }
}
