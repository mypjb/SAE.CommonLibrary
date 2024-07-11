using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SAE.Framework.Abstract.Mediator;
using SAE.Framework.Abstract.Mediator.Behavior;
using SAE.Framework.Abstract.Test.Mediator.Behavior;
using SAE.Framework.Caching;
using SAE.Framework.Test;
using Xunit;
using Xunit.Abstractions;

namespace SAE.Framework.Abstract.Test.Mediator
{
    public class MediatorTest : BaseTest
    {
        private readonly IMediator _mediator;
        private readonly IDistributedCache _distributedCache;

        public MediatorTest(ITestOutputHelper output) : base(output)
        {
            _mediator = this._serviceProvider.GetService<IMediator>();
            this._distributedCache = this._serviceProvider.GetService<IDistributedCache>();
        }
        public override void ConfigureConfiguration(IConfigurationBuilder configurationBuilder)
        {
            base.ConfigureConfiguration(configurationBuilder);
            configurationBuilder.AddInMemoryCollection(new Dictionary<string, string>()
            {
               {$"{RetryPipelineBehaviorOptions.Option}:{nameof(RetryPipelineBehaviorOptions.Num)}","10"}
            });
        }
        protected override void ConfigureServices(IServiceCollection services)
        {
            services.AddSAEFramework()
                    .AddMediator();
            services.AddSingleton(this._output);
            services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(Test1Behavior<,>));
            services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(Test2Behavior<,>));
            services.AddSingleton(typeof(IPipelineBehavior<>), typeof(Test3Behavior<>));
            services.AddSingleton<IPipelineBehavior<SaveCommand, Student>, SaveBehavior>();
            services.AddSingleton<IPipelineBehavior<ChangeCommand>, SaveBehavior>();
            services.AddSAEFramework()
                    .AddMediatorBehavior()
                    .AddCaching<QueryCommand, IEnumerable<Student>>()
                    .AddDeleteCaching<QueryCommand>()
                    .AddUpdateCaching<SaveCommand, IEnumerable<Student>>()
                    .AddDeleteCaching<SaveCommand, Student>()
                    .AddRetry<RetryCommand>()
                    .AddRetry<RetryCommand, Student>();

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
            this.WriteLine(new { time = stopwatch.Elapsed.Ticks, student });
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

        [Fact]
        public async Task CachingPipelineBehavior()
        {
            var command = new QueryCommand
            {
                Begin = 0,
                End = 10
            };

            var key = command.ToString();

            var cacheDatas = await this._distributedCache.GetAsync<IEnumerable<Student>>(key);

            Assert.Null(cacheDatas);

            var students = await this._mediator.SendAsync<IEnumerable<Student>>(command);

            cacheDatas = await this._distributedCache.GetAsync<IEnumerable<Student>>(key);

            this.WriteLine(new { Source = students, Cache = cacheDatas });

            Assert.NotNull(cacheDatas);

            Assert.Contains(students,
                            s => cacheDatas.Any(c => c.Age == s.Age &&
                                                c.Name == s.Name &&
                                                c.Sex == s.Sex &&
                                                c.CreateTime == s.CreateTime));

            await this._mediator.SendAsync(command);

            var deleteCacheDatas = await this._distributedCache.GetAsync<IEnumerable<Student>>(key);

            Assert.Null(deleteCacheDatas);

            var saveCommand = new SaveCommand
            {
                Name = this.GetRandom()
            };

            var saveKey = saveCommand.ToString();

            var saveStudents = await this._mediator.SendAsync<IEnumerable<Student>>(saveCommand);

            var saveCacheDatas = await this._distributedCache.GetAsync<IEnumerable<Student>>(saveKey);

            Assert.Contains(saveStudents,
                            s => saveCacheDatas.Any(c => c.Age == s.Age &&
                                                c.Name == s.Name &&
                                                c.Sex == s.Sex &&
                                                c.CreateTime == s.CreateTime));

            this.WriteLine(new { Source = saveStudents, Cache = saveCacheDatas });

            var student = await this._mediator.SendAsync<Student>(saveCommand);

            var deleteSaveCacheDatas = await this._distributedCache.GetAsync<IEnumerable<Student>>(saveKey);

            Assert.Null(deleteSaveCacheDatas);
        }

        [Fact]
        public async Task Retry()
        {

            var command = new RetryCommand();

            var student = await this._mediator.SendAsync<Student>(command);

            Assert.Equal(10, student.Age);
            command.Number = 1;
            await this._mediator.SendAsync(command);

            Assert.Equal(10, command.Number);

            var exception = await Assert.ThrowsAsync<SAEException>(() => this._mediator.SendAsync(command));
            this.WriteLine(exception.Message);

            exception = await Assert.ThrowsAsync<SAEException>(() => this._mediator.SendAsync<Student>(command));
            this.WriteLine(exception.Message);
        }
    }
}
