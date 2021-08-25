using Microsoft.Extensions.DependencyInjection;
using SAE.CommonLibrary.Abstract.Mediator;
using SAE.CommonLibrary.Abstract.Mediator.Behavior;
using SAE.CommonLibrary.Abstract.Test.Mediator.Behavior;
using SAE.CommonLibrary.Test;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace SAE.CommonLibrary.Abstract.Test.Mediator
{
    public class MediatorTest : BaseTest
    {
        private readonly IMediator _mediator;

        public MediatorTest(ITestOutputHelper output) : base(output)
        {
            _mediator = this._serviceProvider.GetService<IMediator>();
        }
        protected override void ConfigureServices(IServiceCollection services)
        {
            services.AddMediator();
            services.AddSingleton(this._output);
            services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(Test1Behavior<,>));
            services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(Test2Behavior<,>));
            services.AddSingleton(typeof(IPipelineBehavior<>), typeof(Test3Behavior<>));
            services.AddSingleton<IPipelineBehavior<SaveCommand, Student>, SaveBehavior>();
            services.AddSingleton<IPipelineBehavior<ChangeCommand>, SaveBehavior>();
            base.ConfigureServices(services);
        }

        [Fact]
        public async Task Save()
        {
            var defaultName = "test";
            var command = new SaveCommand
            {
                Name = defaultName
            };
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var student = await this._mediator.SendAsync<Student>(command);
            stopwatch.Stop();
            this.WriteLine(new { time = stopwatch.Elapsed.Ticks,student});
            Assert.NotEqual(command.Name, defaultName);
            Assert.True(student.Age > 0);
            Assert.NotEqual(student.Name, command.Name);
            Assert.Equal(Sex.Man, student.Sex);
        }

        [Fact]
        public async Task Change()
        {
            for (int i = 0; i < 10; i++)
            {
                await this.Save();
            }

            var defaultName = "test";

            var command = new ChangeCommand
            {
                Name = defaultName
            };
            await this._mediator.SendAsync(command);

            Assert.NotEqual(command.Name, defaultName);
        }
    }
}
