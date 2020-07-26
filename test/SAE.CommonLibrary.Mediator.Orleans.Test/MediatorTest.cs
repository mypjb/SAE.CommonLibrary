using Microsoft.Extensions.DependencyInjection;
using SAE.CommonLibrary.Abstract.Mediator;
using SAE.CommonLibrary.EventStore.Document;
using SAE.CommonLibrary.Mediator.Orleans.Orders;
using SAE.CommonLibrary.Mediator.Orleans.Product;
using SAE.CommonLibrary.Test;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace SAE.CommonLibrary.Mediator.Orleans.Test
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
            this.Init(typeof(OrderCommand).Assembly);
            this.Init(typeof(ProductCommand).Assembly);

            Thread.Sleep(1000 * 300);
            services.AddMediator()
                    .AddMediatorOrleansClient();
            services.SaeConfigure<OrleansOptions>(options =>
            {
                options.ClusterId = "dev";
            });
            base.ConfigureServices(services);
        }
        private void Init(params Assembly[] assemblies)
        {
            IServiceCollection services = new ServiceCollection();

            this.ConfigureEnvironment(services);

            services.AddMediator(assemblies)
                    .AddMediatorOrleansProxy();

            services.SaeConfigure<OrleansOptions>(options =>
            {
                options.ClusterId = "dev";
            }); ;

            var serviceProvider = services.BuildAutofacProvider()
                                          .UseMediatorOrleansSilo();
        }

        protected override void Configure(IServiceProvider provider)
        {
            base.Configure(provider);
        }

        [Fact]
        public async Task Send()
        {
            var command = new OrderCommand();
            var order = await this._mediator.Send<Order>(command);
            this.WriteLine(order);
        }
        [Fact]
        public async Task SendProduct()
        {
            var command = new ProductCommand();
            var product = await this._mediator.Send<Product.Product>(command);
            this.WriteLine(product);
        }

        [Fact]
        public async Task SendFindCommand()
        {
            var command = new Command.Delete<Order>
            {
                Id = Guid.NewGuid().ToString("N")
            };
            var result = await this._mediator.Send<bool>(command);

            Xunit.Assert.True(result);
        }
    }
}
