using Microsoft.Extensions.DependencyInjection;
using SAE.CommonLibrary.Abstract.Mediator;
using SAE.CommonLibrary.Mediator.Orleans.Orders;
using SAE.CommonLibrary.Mediator.Orleans.Product;
using SAE.CommonLibrary.Test;
using System;
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
            this.Init();

            services.AddMediator(typeof(ProductCommand).Assembly)
                    .AddMediatorOrleansClient();
            services.SaeConfigure<OrleansOptions>(options =>
            {
                options.ClusterId = "dev";
            });
            base.ConfigureServices(services);
        }
        private void Init()
        {
            IServiceCollection services = new ServiceCollection();

            this.ConfigureEnvironment(services);

            services.AddMediator(typeof(OrderCommand).Assembly)
                    .AddMediatorOrleansSilo();

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
            var id=await this._mediator.Send<string>(command);
            Xunit.Assert.Equal(id,command.Id);
        }
    }
}
