using Microsoft.Extensions.DependencyInjection;
using SAE.CommonLibrary.Logging;
using SAE.CommonLibrary.Test;
using System;
using Xunit;
using Xunit.Abstractions;

namespace SAE.CommonLibrary.Logging.Test
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
            services.AddNlogLogger();
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
            _logging.Write($"���ԡ�{type}�����͵���Ϣ", type);
            this._logging.Write($"���ԡ�{type}�����͵���Ϣ", type);
        }
        [Fact]
        public void Info()
        {
            _logging.Info("��¼һ����ϸ��Ϣ");
            this._logging.Info("���{0},����{1},�ҽ���{2}����,-----{3}", "pjb", "cwj", 24, "...");
        }
        [Fact]
        public void Debug()
        {
            _logging.Debug("��¼һ��������Ϣ");
            this._logging.Debug("��¼һ��{0}��Ϣ{1}", "����", "---");
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
                _logging.Error(error, "��¼һ���쳣��Ϣ");
            }

        }
        [Fact]
        public void Fatal()
        {
            this._logging.Error(new Exception("����һ���Զ�����쳣��¼"));
            this._logging.Error("�����ַ�ƴ��");
            _logging.Fatal("��¼һ������������Ϣ");
        }
        [Fact]
        public void Warn()
        {
            _logging.Warn("��¼һ��������Ϣ");
        }
    }
}
