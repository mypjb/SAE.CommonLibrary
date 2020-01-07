using Microsoft.Extensions.DependencyInjection;
using SAE.CommonLibrary.Abstract.Mediator;
using SAE.CommonLibrary.Test;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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
            services.AddSingleton(typeof(ICommandHandler<SaveCommand>), typeof(AddHandler));
            services.AddSingleton(typeof(ICommandHandler<SaveCommand>), typeof(UpdateHandler));
            base.ConfigureServices(services);
        }
        public async Task Send()
        {
            var command = new SaveCommand();
            await this._mediator.Send(command);
        }
    }
}
