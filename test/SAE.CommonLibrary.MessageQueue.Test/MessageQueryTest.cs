using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SAE.CommonLibrary.MessageQueue;
using SAE.CommonLibrary.Test;
using Xunit;
using Xunit.Abstractions;

namespace SAE.CommonLibrary.MessageQueue.Test
{
    public class MessageQueryTest : BaseTest
    {
        private readonly IMessageQueue _messageQueue;
        private readonly CountOptions _options;

        public MessageQueryTest(ITestOutputHelper output) : base(output)
        {
            this._messageQueue = this._serviceProvider.GetService<IMessageQueue>();
            this._options = this._serviceProvider.GetService<CountOptions>();
        }

        protected override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            services.AddMemoryMessageQueue()
                    .AddHandler();
            services.AddSingleton<CountOptions, CountOptions>();
            services.AddSingleton<ITestOutputHelper>(this._output);
        }

        [Fact]
        public async Task PubshSubscibeTest()
        {
            var @event = new AccumulationEvent
            {
                Message = this.GetRandom()
            };
            await this._messageQueue.SubscibeAsync<AccumulationEvent>();
            await this._messageQueue.PublishAsync(@event);
            Thread.Sleep(1000 * 3);
            Assert.Equal(1, this._options.Count);
            await this._messageQueue.SubscibeAsync<AccumulationEvent>(e =>
            {
                this._options.Count += 2;
                return Task.CompletedTask;
            });
            await this._messageQueue.PublishAsync(@event);
            Thread.Sleep(1000 * 3);
            Assert.Equal(3, this._options.Count);
            var identity = "3";
            await this._messageQueue.SubscibeAsync<AccumulationEvent>(identity, e =>
            {
                this._options.Count += 3;
                return Task.CompletedTask;
            });

            Thread.Sleep(1000 * 3);
            await this._messageQueue.PublishAsync(identity, @event);
            Assert.Equal(6, this._options.Count);
        }
    }

    public class CountOptions
    {
        public int Count { get; set; }
    }

    public class AccumulationEvent
    {
        public string Message { get; set; }
    }

    public class CountHandler : IHandler<AccumulationEvent>
    {
        private readonly CountOptions _options;
        private readonly ITestOutputHelper _outputHelper;
        public CountHandler(CountOptions options, ITestOutputHelper outputHelper)
        {
            this._outputHelper = outputHelper;
            this._options = options;

        }
        public Task HandleAsync(AccumulationEvent message)
        {
            this._options.Count += 1;
            this._outputHelper.WriteLine($"{this._options.Count}:{message}");
            return Task.CompletedTask;
        }
    }
}