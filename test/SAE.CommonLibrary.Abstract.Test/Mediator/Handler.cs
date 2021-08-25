using SAE.CommonLibrary.Abstract.Mediator;
using SAE.CommonLibrary.Test;
using System;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Abstract.Test.Mediator
{
    public class AddHandler : ICommandHandler<SaveCommand, Student>
    {
        public Task<Student> HandleAsync(SaveCommand command)
        {
            return Task.FromResult(new Student
            {
                CreateTime = DateTime.Now,
                Sex = Sex.Man
            });
        }
    }

    public class UpdateHandler : ICommandHandler<ChangeCommand, string>
    {
        public Task<string> HandleAsync(ChangeCommand command)
        {
            return Task.FromResult(command.Name+Guid.NewGuid().ToString("N"));
        }
    }
}
