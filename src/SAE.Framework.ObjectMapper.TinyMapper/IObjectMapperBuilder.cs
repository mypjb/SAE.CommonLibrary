using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.Framework.ObjectMapper
{
    public interface IObjectMapperBuilder
    {
        IServiceCollection Services { get; }
        void Add(Action<IObjectMapper> configAction);
        void Build(IObjectMapper objectMapper);
    }

    public class ObjectMapperBuilder: IObjectMapperBuilder
    {
        private Action<IObjectMapper> OnBuilder;
        public ObjectMapperBuilder(IServiceCollection services)
        {
            Services = services;
        }
        public IServiceCollection Services { get; }

        public void Build(IObjectMapper objectMapper)
        {
            OnBuilder?.Invoke(objectMapper);
        }

        public void Add(Action<IObjectMapper> configAction)
        {
            OnBuilder += configAction;
        }
    }
}
