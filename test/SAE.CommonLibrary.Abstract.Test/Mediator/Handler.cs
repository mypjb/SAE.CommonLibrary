using SAE.CommonLibrary.Abstract.Mediator;
using SAE.CommonLibrary.Extension;
using SAE.CommonLibrary.Test;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Abstract.Test.Mediator
{
    public class AddHandler : ICommandHandler<SaveCommand, Student>, ICommandHandler<SaveCommand, IEnumerable<Student>>
    {
        public Task<Student> HandleAsync(SaveCommand command)
        {
            return Task.FromResult(new Student
            {
                CreateTime = DateTime.Now,
                Sex = Sex.Man
            });
        }

        Task<IEnumerable<Student>> ICommandHandler<SaveCommand, IEnumerable<Student>>.HandleAsync(SaveCommand command)
        {
            var students = new[]
            {
                new Student
                {
                    CreateTime = DateTime.Now,
                    Sex = Sex.Man,
                    Name=command.Name,
                    Age=100
                }
            }.AsEnumerable();
            return Task.FromResult(students);
        }
    }

    public class UpdateHandler : ICommandHandler<ChangeCommand, string>
    {
        public Task<string> HandleAsync(ChangeCommand command)
        {
            return Task.FromResult(command.Name + Guid.NewGuid().ToString("N"));
        }
    }

    public class QueryHandler : ICommandHandler<QueryCommand, IEnumerable<Student>>,
                                ICommandHandler<QueryCommand>
    {
        public async Task<IEnumerable<Student>> HandleAsync(QueryCommand command)
        {
            return Enumerable.Range(command.Begin, command.End - command.Begin)
                      .Select(s => new Student
                      {
                          Age = s,
                          CreateTime = DateTime.Now,
                          Name = s.ToString(),
                          Sex = s % 2 == 0 ? Sex.Nav : Sex.Man
                      }).ToArray();
        }

        Task ICommandHandler<QueryCommand>.HandleAsync(QueryCommand command)
        {
            return Task.CompletedTask;
        }
    }
}
