using CaiXin.NiuMa.Application.Contracts.MemberApp.Eto;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;

namespace CaiXin.NiuMa.Application.MemberApp.Subscribe
{
    /// <summary>
    /// Cap 事件订阅
    /// </summary>
    /// <param name="_logger"></param>
    public class Class1(ILogger<Class1> _logger) : ICapSubscribe, ITransientDependency
    {
        //订阅事件
        [NonAction]
        [CapSubscribe("test")]
        public async Task<string> TestCapSubscribe(MemberRegistrationEtos message)
        {
            //await Task.Delay(3000);
            _logger.LogInformation($"cap订阅事件已经完毕：{message}");
            return "OK";
        }
    }
}