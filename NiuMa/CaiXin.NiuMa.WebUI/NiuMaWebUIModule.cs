using CaiXin.NiuMa.Application;
using CaiXin.NiuMa.Application.Contracts;
using CaiXin.NiuMa.Domain.Shared.Response;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.OpenApi.Models;
using NUglify.Helpers;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Net;
using System.Reflection;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace CaiXin.NiuMa.WebUI
{


    /// <summary>
    /// WebModule
    /// </summary>
    [DependsOn(
             typeof(AbpAspNetCoreMvcModule),
             typeof(AbpAutofacModule),
             typeof(NiuMaApplicationContractsModule),
             typeof(NiuMaApplicationModule)
    )]
    public class NiuMaWebUIModule : AbpModule
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task ConfigureServicesAsync(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();



            // context.Services.AddFluentValidationClientsideAdapters();
            context.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            context.Services.AddControllersWithViews(opt =>
            {
                opt.ModelValidatorProviders.Clear();
                //opt.Filters.Add(typeof(ResultExceptionFilter));
                opt.EnableEndpointRouting = false;
            }).AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix);





            context.Services.AddHttpContextAccessor();



            var domainName = configuration.GetSection("DomainName").Get<string>();
            context.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                 .AddCookie(options =>
                 {
                     options.LoginPath = "/";
                     options.AccessDeniedPath = "/";
                     options.Cookie.Name = "AppendRoomUser";
                     options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                     options.SlidingExpiration = true;
                     options.Events = new CookieAuthenticationEvents
                     {
                         OnRedirectToLogin = context =>
                         {
                             // 获取 IHttpContextAccessor
                             var httpContextAccessor = context.HttpContext.RequestServices.GetRequiredService<IHttpContextAccessor>();
                             // 获取当前请求的域名和协议
                             var request = context.HttpContext.Request;
                             var host = request.Host.Value;
                             var scheme = request.Scheme; // http 或 https

                             var baseUrl = domainName ?? $"{scheme}://{host}";


                             context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                             if (context.Request.Headers.ContainsKey("Accept") && (context.Request.Headers["Accept"].FirstOrDefault()?.Contains("application/json") ?? false))
                             {
                                 context.Response.ContentType = "application/json";
                                 context.Response.StatusCode = 401;
                                 context.Response.WriteAsJsonAsync(new ApiResult()
                                 {
                                     Code = 401,
                                     Message = "登录已失效，请重新登录"
                                 });
                             }
                             else
                             {
                                 context.Response.WriteAsync($"<script>top.location.href='{baseUrl}';</script>");
                             }
                             return Task.CompletedTask;
                         }
                     };
                 });






            context.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "YueJiaAppendRoom API", Version = "v1" });
                Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "CaiXin.NiuMa.*.xml")
                 .Select(x => x)
                 .ForEach(item => c.IncludeXmlComments(item, true));
            });

            //context.Services.Configure<SmsConfig>(context.Services.GetConfiguration().GetSection("Sms"));
            //context.Services.Configure<EmailConfig>(context.Services.GetConfiguration().GetSection("Email"));
            //context.Services.Configure<SysErrorEmailRecipientConfig>(context.Services.GetConfiguration().GetSection("SysErrorEmailRecipient"));
            return base.ConfigureServicesAsync(context);
        }



        public override Task OnPreApplicationInitializationAsync(ApplicationInitializationContext context)
        {
            return base.OnPreApplicationInitializationAsync(context);
        }
        public override Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "YueEbk API");
                options.DefaultModelsExpandDepth(-1);

                options.DocExpansion(DocExpansion.None);
            });
            return base.OnApplicationInitializationAsync(context);
        }

    }
}
