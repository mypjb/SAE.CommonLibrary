using SAE.CommonLibrary.Abstract.Mediator;
using SAE.CommonLibrary.Extension;
using SAE.CommonLibrary.Mediator.Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SaeMediatorOrleansDependencyInjectionExtension
    {
        public static IMediatorBuilder AddOrleansProxy(this IMediatorBuilder builder)
        {
            var identitys = new Dictionary<string,Assembly>();

            builder.Descriptors.ForEach(descriptor =>
            {
                var identity = Utility.GetIdentity(descriptor.CommandType);
                if (!identitys.ContainsKey(identity))
                {
                    identitys.Add(identity, descriptor.CommandType.Assembly);
                }
            });

            builder.Services.AddSaeOptions<OrleansOptions>()
                            .SaeConfigure<OrleansOptions>(options =>
                            {
                                foreach (var kv in identitys)
                                    options.GrainNames.TryAdd(kv.Key, kv.Value);
                            });
            
            return builder;
        }

        public static IServiceProvider UseMediatorProxy(this IServiceProvider provider)
        {
            //var descriptors = provider.GetService<IEnumerable<CommandHandlerDescriptor>>();
            
            return provider;
        }
    }
}
