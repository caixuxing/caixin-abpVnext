using CaiXin.NiuMa.Application.Contracts.MemberApp.Eto;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;

namespace CaiXin.NiuMa.Application.MemberApp.Subscribe
{
    public class Class1(ILogger<Class1> _logger) : ICapSubscribe, ITransientDependency
    {
        //订阅事件
        [NonAction]
        [CapSubscribe("test")]
        public async Task<string> TestCapSubscribe(MemberRegistrationEtos message)
        {
            await Task.Delay(3000);
            _logger.LogInformation($"cap订阅事件已经完毕：{message}");
            return "OK";
        }
    }
}
