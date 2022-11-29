using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SAE.CommonLibrary.Extension;
using Xunit;
using Xunit.Abstractions;

namespace SAE.CommonLibrary.Scope.Test;

public class DefaultScopeTest : SAE.CommonLibrary.Test.BaseTest
{
    private readonly IScopeFactory _scopeFactory;

    public DefaultScopeTest(ITestOutputHelper output) : base(output)
    {
        this._scopeFactory = this._serviceProvider.GetService<IScopeFactory>();
    }

    protected override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);
        services.AddDefaultScope();
    }

    [Fact]
    public Task ScopeSwitch()
    {
        Enumerable.Range(0, new Random().Next(999, 9999))
                  .Select(s => s.ToString())
                  .ToArray()
                  .AsParallel()
                  .ForAll(async s =>
                  {
                      using (var scope = await this._scopeFactory.GetAsync(s))
                      {
                          Xunit.Assert.Equal(scope.Name, s);
                      }
                  });
        this.WriteLine(1111);
        return Task.CompletedTask;
    }

    
}