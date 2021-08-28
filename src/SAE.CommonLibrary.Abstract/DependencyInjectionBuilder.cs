using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Abstract
{
    public abstract class DependencyInjectionBuilder
    {
        public DependencyInjectionBuilder(IServiceCollection services)
        {
            Services = services;
        }
        public IServiceCollection Services { get; }
    }
}
