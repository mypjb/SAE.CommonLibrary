using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary.Abstract.Mediator
{
    public interface IMediatorBuilder
    {
        public IEnumerable<CommandHandlerDescriptor> Descriptors { get; }
        public IServiceCollection Services{ get;}
    }
    public class MediatorBuilder : IMediatorBuilder
    {
        private List<CommandHandlerDescriptor> descriptors;


        public MediatorBuilder(IServiceCollection services, List<CommandHandlerDescriptor> descriptors)
        {
            this.Services = services;
            this.Descriptors = descriptors;
        }

        public IServiceCollection Services { get; }

        public IEnumerable<CommandHandlerDescriptor> Descriptors { get; }
    }
}
