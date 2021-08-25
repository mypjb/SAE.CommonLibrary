using SAE.CommonLibrary.Abstract.Mediator.Behavior;
using SAE.CommonLibrary.Extension;
using SAE.CommonLibrary.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace SAE.CommonLibrary.Abstract.Test.Mediator.Behavior
{
    public class SaveBehavior : IPipelineBehavior<SaveCommand, Student>, IPipelineBehavior<ChangeCommand>, IOrdered
    {
        private readonly ITestOutputHelper _testOutputHelper;
        public SaveBehavior(ITestOutputHelper testOutputHelper)
        {
            this.Order = 999;
            this._testOutputHelper = testOutputHelper;
        }

        public int Order { get;  }

        public async Task<Student> ExecutionAsync(SaveCommand command, Func<Task<Student>> next)
        {
            command.Name += Guid.Empty.ToString("N");
            var student = await next();
            student.Age = 100;
            return student;
        }

        public async Task ExecutionAsync(ChangeCommand command, Func<Task> next)
        {
            command.Name += Guid.Empty.ToString("N");
            await next();
        }
    }
}
