using Microsoft.Extensions.DependencyInjection.Extensions;
using SAE.CommonLibrary.Abstract.Mediator;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtension
    {
        /// <summary>
        /// 添加中介者
        /// </summary>
        /// <param name="serviceDescriptors"></param>
        /// <returns></returns>
        public static IServiceCollection AddMediator(this IServiceCollection serviceDescriptors)
        {
            serviceDescriptors.TryAddSingleton<IMediator, Mediator>();
            return serviceDescriptors;
        }
    }
}
