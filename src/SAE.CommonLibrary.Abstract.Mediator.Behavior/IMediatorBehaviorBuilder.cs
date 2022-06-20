using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Abstract.Mediator.Behavior
{
    public interface IMediatorBehaviorBuilder
    {
        public IServiceCollection Services { get; }
    }

    public class MediatorBehaviorBuilder : DependencyInjectionBuilder, IMediatorBehaviorBuilder
    {
        public MediatorBehaviorBuilder(IServiceCollection services) : base(services)
        {
        }
    }
}
