using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace SAE.CommonLibrary.Abstract.Mediator
{
    public class Mediator : IMediator
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly ConcurrentDictionary<string, object> _dic;
        public Mediator(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
            this._dic = new ConcurrentDictionary<string, object>();
        }


        public async Task<object> SendAsync(object command, Type commandType, Type responseType)
        {
            var key = $"{commandType}_{responseType}";

            var wrapper = this._dic.GetOrAdd(key, k =>
            {
                return Activator.CreateInstance(typeof(RequestHandlerWrapper<,>)
                                .MakeGenericType(commandType, responseType), this._serviceProvider);
            });

            var response = await((RequestHandlerWrapper)wrapper).InvokeAsync(command);

            return response;
        }

        public async Task SendAsync(object command, Type commandType)
        {
            var key = commandType.ToString();

            var wrapper = this._dic.GetOrAdd(key, k =>
            {
                return Activator.CreateInstance(typeof(CommandHandlerWrapper<>)
                                .MakeGenericType(commandType), this._serviceProvider);
            });

            await((CommandHandlerWrapper)wrapper).InvokeAsync(command);
        }
    }
}
