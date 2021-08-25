using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Abstract.Mediator.Behavior
{
    public interface IPipelineBehavior<TCommand, TResponse> where TCommand : class
    {
        public Task<TResponse> ExecutionAsync(TCommand command, Func<Task<TResponse>> next);
    }

    public interface IPipelineBehavior<TCommand> where TCommand : class
    {
        public Task ExecutionAsync(TCommand command, Func<Task> next);
    }
}
