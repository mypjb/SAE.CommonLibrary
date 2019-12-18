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
            //LogHelper静态函数写入各种层次的信息
            _logging.Write($"测试“{type}”类型的消息", type);
            //使用LogHelperTest类型的日志记录器写入信息
            this._logging.Write($"测试“{type}”类型的消息", type);
        }
        [Fact]
        public void Info()
        {
            _logging.Info("记录一条详细信息");
            this._logging.Info("你好{0},我是{1},我今年{2}岁了,-----{3}", "pjb", "cwj", 24, "...");
        }
        [Fact]
        public void Debug()
        {
            _logging.Debug("记录一条调试信息");
            this._logging.Debug("记录一条{0}信息{1}", "调试", "---");
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
                _logging.Error(error, "记录一条异常信息");
            }

        }
        [Fact]
        public void Fatal()
        {
            this._logging.Error(new Exception("这是一个自定义的异常记录"));
            this._logging.Error("这是字符拼接");
            _logging.Fatal("记录一条致命错误信息");
        }
        [Fact]
        public void Warn()
        {
            _logging.Warn("记录一条警告信息");
            //this._logging.WarnFormat("呵呵达。");
            //LogHelper.Warn<object>($"记录一条{nameof(Object)}类型的的警告信息");
        }
    }
}
