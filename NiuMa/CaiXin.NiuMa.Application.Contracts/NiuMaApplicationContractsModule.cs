using FluentValidation;
using System.Reflection;
using Volo.Abp.Modularity;

namespace CaiXin.NiuMa.Application.Contracts
{
    public class NiuMaApplicationContractsModule : AbpModule
    {
        public override Task ConfigureServicesAsync(ServiceConfigurationContext context)
        {
            // context.Services.AddFluentValidationClientsideAdapters();
            // context.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            context.Services.AddValidatorsFromAssembly(typeof(AssemblyMarker).Assembly);
            return base.ConfigureServicesAsync(context);
        }
    }
}