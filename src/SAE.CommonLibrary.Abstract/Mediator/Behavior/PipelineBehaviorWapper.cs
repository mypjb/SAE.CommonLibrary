using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Abstract.Mediator.Behavior
{
    internal class PipelineBehaviorWapper<TCommand, TResponse> where TCommand : class
    {
        public PipelineBehaviorWapper(TCommand command,
                                      IPipelineBehavior<TCommand, TResponse> proxy,
                                      Func<Task<TResponse>> next)
        {
            this.Command = command;
            this.Proxy = proxy;
            this.Next = next;
        }
        public IPipelineBehavior<TCommand, TResponse> Proxy { get; }
        public TCommand Command { get; }
        public Func<Task<TResponse>> Next { get; }

        public Func<Task<TResponse>> Build()
        {
            return () => this.Proxy.ExecutionAsync(this.Command,this.Next);
        }
    }

    internal class PipelineBehaviorWapper<TCommand> where TCommand : class
    {
        public PipelineBehaviorWapper(TCommand command,
                                      IPipelineBehavior<TCommand> proxy,
                                      Func<Task> next)
        {
            this.Command = command;
            this.Proxy = proxy;
            this.Next = next;
        }
        public IPipelineBehavior<TCommand> Proxy { get; }
        public TCommand Command { get; }
        public Func<Task> Next { get; }

        public Func<Task> Build()
        {
            return () => this.Proxy.ExecutionAsync(this.Command, this.Next);
        }
    }
}
