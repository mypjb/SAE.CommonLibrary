using Microsoft.Extensions.DependencyInjection;
using SAE.Framework.Logging;
using SAE.Framework.Logging.Nlog;
using SAE.Framework.Test;
using System;
using Xunit;
using Xunit.Abstractions;

namespace SAE.Framework.Logging.Test
{
    public class NlogTest : BaseTest
    {
        private readonly ILogging<NlogTest> _logging;

        public NlogTest(ITestOutputHelper output) : base(output)
        {
            this._logging = this._serviceProvider.GetService<ILogging<NlogTest>>();
        }
        protected override void ConfigureServices(IServiceCollection services)
        {
            services.AddSAEFramework()
                    .AddNlogLogger();
            base.ConfigureServices(services);
        }
        [Theory]
        [InlineData(Level.Debug)]
        [InlineData(Level.Error)]
        [InlineData(Level.Fatal)]
        [InlineData(Level.Info)]
        [InlineData(Level.Warn)]
        [InlineData(Level.Trace)]
        public void Write(Level type)
        {
            _logging.Write($"this level is {type}", type);
        }
        [Fact]
        public void Info()
        {
            this._logging.Info("this is info output");
            this._logging.Info("1.{0},2.{1},3.{2},-----4.{3}", "pjb", "cwj", 24, "...");
        }
        [Fact]
        public void Debug()
        {
            _logging.Debug("this is debug output");
            this._logging.Debug("1.{0}2.{1}", "pjb", "---");
        }
        [Fact]
        public void Error()
        {
            var ex = new ArgumentNullException(paramName: "Student.Name");
            try
            {
                throw ex;
            }
            catch (Exception error)
            {
                _logging.Error(error, "this is error output");
            }

        }
        [Fact]
        public void Fatal()
        {
            this._logging.Error(new Exception("this is custom error"),"custom error");
            this._logging.Error("this is error output");
            this._logging.Fatal("this is Fatal output");
        }
        [Fact]
        public void Warn()
        {
            this._logging.Warn("this is Warn output");
        }
    }
}
