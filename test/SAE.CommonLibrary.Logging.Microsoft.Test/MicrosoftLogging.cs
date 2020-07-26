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
            this._logger.LogInformation("��¼һ����ϸ��Ϣ");
            this._logger.LogInformation("���{0},����{1},�ҽ���{2}����,-----{3}", "pjb", "cwj", 24, "...");
            this.Check();
        }
        [Fact]
        public void Debug()
        {
            _logger.LogDebug("��¼һ��������Ϣ");
            this._logger.LogDebug("��¼һ��{0}��Ϣ{1}", "����", "---");
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
                _logger.LogError(error, "��¼һ���쳣��Ϣ");
            }
            this.Check();
        }
        [Fact]
        public void Critical()
        {
            this._logger.LogError(new Exception("����һ���Զ�����쳣��¼"), "错误异常");
            this._logger.LogError("�����ַ�ƴ��");
            _logger.LogCritical("��¼һ������������Ϣ");
            this.Check();
        }
        [Fact]
        public void Warn()
        {
            _logger.LogError("��¼һ��������Ϣ");
            this.Check();
        }
    }
}
