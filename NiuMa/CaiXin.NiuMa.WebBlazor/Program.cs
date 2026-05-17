using CaiXin.NiuMa.WebBlazor;
using CaiXin.NiuMa.WebBlazor.Components;

var builder = WebApplication.CreateBuilder(args);

//日志配置
//builder.Logging.ClearProviders();

builder.Host.UseAutofac();
//配置文件
builder.Services.ReplaceConfiguration(builder.Configuration);
//基础组件注入
await builder.Services.AddApplicationAsync<NiuMaWebBlazorModule>();

var app = builder.Build();
await app.InitializeApplicationAsync();

app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

await app.RunAsync();