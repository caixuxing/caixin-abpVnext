using CaiXin.NiuMa.WebBlazor;

var builder = WebApplication.CreateBuilder(args);

//日志配置
builder.Logging.ClearProviders();
//Log.Logger = new LoggerConfiguration()
//          .Enrich.FromLogContext()
//          .WriteTo.Console()// 日志输出到控制台
//          .WriteTo.File($"data/logs/log-.txt", restrictedToMinimumLevel: LogEventLevel.Warning, rollingInterval: RollingInterval.Day)
//          .MinimumLevel.Information()
//          .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
//          .CreateLogger();
builder.Host.UseAutofac();
//配置文件
builder.Services.ReplaceConfiguration(builder.Configuration);//修正配置错误
//基础组件注入
builder.Services.AddApplication<NiuMaWebBlazorModule>();
builder.Services.AddHttpClient();

var app = builder.Build();
await app.InitializeApplicationAsync();
await app.RunAsync();