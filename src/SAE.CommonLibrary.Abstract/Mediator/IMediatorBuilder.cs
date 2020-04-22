using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary.Abstract.Mediator
{
    public interface IMediatorBuilder
    {
        public IServiceCollection Services{ get;}
    }
    public class MediatorBuilder : IMediatorBuilder
    {
        public MediatorBuilder(IServiceCollection services)
        {
            this.Services = services;
        }
        public IServiceCollection Services { get; }
    }
}
