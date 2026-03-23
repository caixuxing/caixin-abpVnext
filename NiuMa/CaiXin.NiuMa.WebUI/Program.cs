using Serilog;

namespace CaiXin.NiuMa.WebUI;

/// <summary>
/// Program 类
/// </summary>
public static class Program
{
    /// <summary>
    /// 程序入口
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    public static async Task Main(string[] args)
    {

        try
        {
            var builder = WebApplication.CreateBuilder(args);

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("Config/Serilog.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"Config/Serilog.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true)
                .AddEnvironmentVariables()
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithEnvironmentUserName()
                .Enrich.WithThreadId()
                .CreateLogger();

            builder.Host.UseSerilog(dispose: true).AddAppSettingsSecretsJson().UseAutofac();
            await builder.AddApplicationAsync<NiuMaWebUIModule>();
            var app = builder.Build();
            await app.InitializeApplicationAsync();
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            await app.RunAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"启动失败！{ex.Message},{ex}");
            Console.WriteLine("按任意键退出...");
            Console.ReadKey();
        }
    }
}
