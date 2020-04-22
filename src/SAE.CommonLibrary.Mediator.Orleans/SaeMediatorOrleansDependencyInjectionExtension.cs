using SAE.CommonLibrary.Abstract.Mediator;
using SAE.CommonLibrary.Mediator.Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SaeMediatorOrleansDependencyInjectionExtension
    {
        public static IMediatorBuilder AddOrleansProxy(this IMediatorBuilder builder)
        {
            builder.Services.AddSaeOptions<OrleansOptions>();
            
            return builder;
        }

        public static IServiceProvider UseMediatorProxy(this IServiceProvider provider)
        {
            //var descriptors = provider.GetService<IEnumerable<CommandHandlerDescriptor>>();
            
            return provider;
        }
    }
}
