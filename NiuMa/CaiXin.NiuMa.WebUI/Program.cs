using Serilog;

namespace CaiXin.NiuMa.WebUI;

/// <summary>
/// 应用程序入口类
/// 负责配置和启动Web应用程序
/// </summary>
public static class Program
{
    /// <summary>
    /// 应用程序主入口点
    /// 配置依赖注入、中间件管道并启动Web服务器
    /// </summary>
    /// <param name="args">命令行参数</param>
    /// <returns>异步任务</returns>
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

            builder.Host
                .UseSerilog(dispose: true)
                .AddAppSettingsSecretsJson()
                .UseAutofac();

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
            Log.Fatal(ex, "应用程序启动失败");
            Console.WriteLine($"启动失败：{ex.Message}");
            Console.WriteLine("应用程序异常退出...");
        }
        finally
        {
           await Log.CloseAndFlushAsync();
        }
    }
}
