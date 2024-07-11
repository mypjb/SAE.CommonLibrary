using SAE.Framework.Abstract.Mediator.Behavior;
using SAE.Framework.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace SAE.Framework.Abstract.Test.Mediator.Behavior
{
    public class Test3Behavior<TCommand> : IOrdered, IPipelineBehavior<TCommand> where TCommand : class
    {
        private readonly ITestOutputHelper testOutputHelper;

        public Test3Behavior(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
            this.Order = 99;
        }

        public int Order { get; }

        public async Task ExecutionAsync(TCommand command, Func<Task> next)
        {
            await next();
        }

    }
}
