//using Serilog;

namespace CaiXin.NiuMa.Host
{
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
                builder.Host.AddAppSettingsSecretsJson().UseAutofac();
                await builder.AddApplicationAsync<NiuMaHostModule>();
                var app = builder.Build();
                await app.InitializeApplicationAsync();
                await app.RunAsync();
            }
            catch (Exception ex)
            {
                //Log.Fatal(ex, "应用程序启动失败");
                Console.WriteLine($"启动失败：{ex.Message}");
                Console.WriteLine("应用程序异常退出...");
            }
            finally
            {
                //await Log.CloseAndFlushAsync();
            }
        }

    }


}
