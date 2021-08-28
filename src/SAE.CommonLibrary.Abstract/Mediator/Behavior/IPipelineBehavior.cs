using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Abstract.Mediator.Behavior
{
    [Obsolete("This interface is only used for marking")]
    public interface IPipelineBehavior
    {

    }
    /// <summary>
    /// Mediator pipeline behavior
    /// </summary>
    /// <typeparam name="TCommand"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public interface IPipelineBehavior<TCommand, TResponse>: IPipelineBehavior where TCommand : class
    {
        public Task<TResponse> ExecutionAsync(TCommand command, Func<Task<TResponse>> next);
    }
    /// <summary>
    /// Mediator pipeline behavior
    /// </summary>
    /// <typeparam name="TCommand"></typeparam>
    public interface IPipelineBehavior<TCommand>: IPipelineBehavior where TCommand : class
    {
        public Task ExecutionAsync(TCommand command, Func<Task> next);
    }
}
