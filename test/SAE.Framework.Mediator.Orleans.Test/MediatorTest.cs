using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SAE.Framework.Abstract.Mediator;
using SAE.Framework.EventStore.Document;
using SAE.Framework.Mediator.Orleans.Orders;
using SAE.Framework.Mediator.Orleans.Product;
using SAE.Framework.Test;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace SAE.Framework.Mediator.Orleans.Test
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
            //this.Init(typeof(ProductCommand).Assembly);

            //Thread.Sleep(1000 * 300);
            services.AddSAEFramework()
                    .AddMediator()
                    .AddMediatorOrleansClient();
            services.PostConfigure<OrleansOptions>(options =>
            {
                options.ClusterId = "dev";
            });
            base.ConfigureServices(services);
        }
        private void Init(params Assembly[] assemblies)
        {
            IServiceCollection services = new ServiceCollection();
            
            services.AddSAEFramework()
                    .AddMediator(assemblies)
                    .AddMediatorOrleansProxy();

            services.PostConfigure<OrleansOptions>(options =>
            {
                options.ClusterId = "dev";
            }); ;

            var serviceProvider = this.Build(services).UseMediatorOrleansSilo();
        }

        protected override void Configure(IServiceProvider provider)
        {
            base.Configure(provider);
        }

        //[Fact]
        public async Task Send()
        {
            var command = new OrderCommand();
            var order = await this._mediator.SendAsync<Order>(command);
            this.WriteLine(order);
        }
        //[Fact]
        //public async Task SendProduct()
        //{
        //    var command = new ProductCommand();
        //    var product = await this._mediator.Send<Product.Product>(command);
        //    this.WriteLine(product);
        //}

        //[Fact]
        public async Task SendFindCommand()
        {
            var command = new Command.Delete<Order>
            {
                Id = Guid.NewGuid().ToString("N")
            };
            var result = await this._mediator.SendAsync<bool>(command);

            Xunit.Assert.True(result);
        }
    }
}
