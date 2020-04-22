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

        public async Task Send(object command)
        {
            var commandType = command.GetType();

            var key = commandType.GUID.ToString();

            var wrapper = this._dic.GetOrAdd(key, k =>
            {
                return Activator.CreateInstance(typeof(CommandHandlerWrapper<>)
                                .MakeGenericType(commandType), this._serviceProvider);
            });

            await ((CommandHandlerWrapper)wrapper).Invoke(command);
        }

        public async Task<object> Send(object command, Type responseType)
        {
            var commandType = command.GetType();
            var key = $"{commandType.GUID}_{responseType.GUID}";

            var wrapper = this._dic.GetOrAdd(key, k =>
            {
                return Activator.CreateInstance(typeof(RequestHandlerWrapper<,>)
                                .MakeGenericType(commandType, responseType), this._serviceProvider);
            });

            var response = await((RequestHandlerWrapper)wrapper).Invoke(command);

            return response;
        }
    }
}
