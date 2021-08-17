using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Fluent;
using SAE.CommonLibrary.Test;
using System;
using System.IO;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace SAE.CommonLibrary.Logging.Test
{
    public class MicrosoftLogging : BaseTest
    {
        private readonly ILogger<MicrosoftLogging> _logger;
        public const string LogFile = "logs";
        public MicrosoftLogging(ITestOutputHelper output) : base(output)
        {
            this._logger = this._serviceProvider.GetService<ILogger<MicrosoftLogging>>();
            if (Directory.Exists(LogFile))
            {
                Directory.Delete(LogFile, true);
            }
        }


        protected override void ConfigureServices(IServiceCollection services)
        {
            services.AddNlogLogger()
                    .AddLogging(config=>
                    {
                        config.SetMinimumLevel(LogLevel.Trace);
                    })
                    .AddMicrosoftLogging();

            base.ConfigureServices(services);
        }

        private void Check()
        {
            var isExist = false;
            if (Directory.Exists(LogFile))
            {
                isExist = Directory.GetFiles(LogFile, "*.log").Count() > 0;
            }

            Xunit.Assert.True(isExist);
        }

        [Fact]
        public void Info()
        {
            this._logger.LogInformation("this is info output");
            this._logger.LogInformation("1.{0},2.{1},3.{2},-----4.{3}", "pjb", "cwj", 24, "...");
            this.Check();
        }
        [Fact]
        public void Debug()
        {
            _logger.LogDebug("this is debug output");
            this._logger.LogDebug("1.{0}2.{1}", "pjb", "---");
            this.Check();
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
                _logger.LogError(error, "this is error output");
            }
            this.Check();
        }
        [Fact]
        public void Critical()
        {
            this._logger.LogError(new Exception("this is custom error"), "错误异常");
            this._logger.LogError("this is error output");
            _logger.LogCritical("this is Critical output");
            this.Check();
        }
        [Fact]
        public void Warn()
        {
            _logger.LogError("this is Warn output");
            this.Check();
        }
    }
}
