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
    public class Test2Behavior<TCommand, TResponse> : IPipelineBehavior<TCommand, TResponse> where TCommand : class
    {
        private readonly ITestOutputHelper testOutputHelper;

        public Test2Behavior(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }
        public async Task<TResponse> ExecutionAsync(TCommand command, Func<Task<TResponse>> next)
        {
            return await next();
        }

    }
}
