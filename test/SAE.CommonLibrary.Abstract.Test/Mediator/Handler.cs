using SAE.CommonLibrary.Abstract.Mediator;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Abstract.Test.Mediator
{
    public class AddHandler : ICommandHandler<SaveCommand>
    {
        public Task Handle(SaveCommand command)
        {
            return Task.CompletedTask;
        }
    }

    public class UpdateHandler : ICommandHandler<SaveCommand>
    {
        public Task Handle(SaveCommand command)
        {
            return Task.FromResult(command);
        }
    }
}
