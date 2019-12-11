using SAE.CommonLibrary.Extension;
using System;
using System.Linq;

namespace SAE.CommonLibrary.Logging
{
    /// <summary>
    /// 日志扩展
    /// </summary>
    public static class LoggingExtension
    {

        #region Debug
        /// <summary>
        /// 记录<see cref="Level.Debug"/>级别日志
        /// </summary>
        /// <param name="log">日志接口</param>
        /// <param name="message">格式化的字符串</param>
        public static ILogging Debug(this ILogging log, string message)
            => log.Write(Level.Debug, message);


        /// <summary>
        /// 记录<see cref="Level.Debug"/>级别日志
        /// </summary>
        /// <param name="log">日志接口</param>
        /// <param name="format">格式化的字符串</param>
        /// <param name="args">参数集合</param>
        public static ILogging Debug(this ILogging log, string format, params object[] args)
            => log.Write(Level.Debug, format, args);

        /// <summary>
        /// 记录<see cref="Level.Debug"/>级别日志
        /// </summary>
        /// <param name="log">日志接口</param>
        /// <param name="provider">字符串格式化提供程序</param>
        /// <param name="format">格式化的字符串</param>
        /// <param name="args">参数集合</param>
        public static ILogging Debug(this ILogging log, IFormatProvider provider, string format, params object[] args)
            => log.Write(Level.Debug, provider, format, args);
        #endregion

        #region Info

        /// <summary>
        /// 记录<see cref="Level.Info"/>级别日志
        /// </summary>
        /// <param name="log">日志接口</param>
        /// <param name="message">格式化的字符串</param>
        public static ILogging Info(this ILogging log, string message)
            => log.Write(Level.Info, message);


        /// <summary>
        /// 记录<see cref="Level.Info"/>级别日志
        /// </summary>
        /// <param name="log">日志接口</param>
        /// <param name="format">格式化的字符串</param>
        /// <param name="args">参数集合</param>
        public static ILogging Info(this ILogging log, string format, params object[] args)
            => log.Write(Level.Info, format, args);

        /// <summary>
        /// 记录<see cref="Level.Info"/>级别日志
        /// </summary>
        /// <param name="log">日志接口</param>
        /// <param name="provider">字符串格式化提供程序</param>
        /// <param name="format">格式化的字符串</param>
        /// <param name="args">参数集合</param>
        public static ILogging Info(this ILogging log, IFormatProvider provider, string format, params object[] args)
            => log.Write(Level.Info, provider, format, args);

        #endregion

        #region Warn
        /// <summary>
        /// 记录<see cref="Level.Warn"/>级别日志
        /// </summary>
        /// <param name="log">日志接口</param>
        /// <param name="message">格式化的字符串</param>
        public static ILogging Warn(this ILogging log, string message)
            => log.Write(Level.Warn, message);


        /// <summary>
        /// 记录<see cref="Level.Warn"/>级别日志
        /// </summary>
        /// <param name="log">日志接口</param>
        /// <param name="format">格式化的字符串</param>
        /// <param name="args">参数集合</param>
        public static ILogging Warn(this ILogging log, string format, params object[] args)
            => log.Write(Level.Warn, format, args);

        /// <summary>
        /// 记录<see cref="Level.Warn"/>级别日志
        /// </summary>
        /// <param name="log">日志接口</param>
        /// <param name="provider">字符串格式化提供程序</param>
        /// <param name="format">格式化的字符串</param>
        /// <param name="args">参数集合</param>
        public static ILogging Warn(this ILogging log, IFormatProvider provider, string format, params object[] args)
            => log.Write(Level.Warn, provider, format, args);
        #endregion

        #region Error

        /// <summary>
        /// 记录<see cref="Level.Error"/>级别日志
        /// </summary>
        /// <param name="log">日志接口</param>
        /// <param name="exception">异常</param>
        public static ILogging Error(this ILogging log, Exception exception)
            => log.Write(exception?.Message, Level.Error, exception: exception);

        /// <summary>
        /// 记录<see cref="Level.Error"/>级别日志
        /// </summary>
        /// <param name="log">日志接口</param>
        /// <param name="message">格式化的字符串</param>
        public static ILogging Error(this ILogging log, string message)
            => log.Write(Level.Error, message);

        /// <summary>
        /// 记录<see cref="Level.Error"/>级别日志
        /// </summary>
        /// <param name="log">日志接口</param>
        /// <param name="exception"></param>
        /// <param name="message">格式化的字符串</param>
        public static ILogging Error(this ILogging log, Exception exception, string message)
            => log.Write(message, Level.Error, exception: exception);

        /// <summary>
        /// 记录<see cref="Level.Error"/>级别日志
        /// </summary>
        /// <param name="log">日志接口</param>
        /// <param name="format">格式化的字符串</param>
        /// <param name="args">参数集合</param>
        public static ILogging Error(this ILogging log, string format, params object[] args)
            => log.Write(Level.Error, format, args);

        /// <summary>
        /// 记录<see cref="Level.Error"/>级别日志
        /// </summary>
        /// <param name="log">日志接口</param>
        /// <param name="exception"></param>
        /// <param name="format">格式化的字符串</param>
        /// <param name="args">参数集合</param>
        public static ILogging Error(this ILogging log, Exception exception, string format, params object[] args)
            => log.Write(Level.Error, exception: exception, provider: null, format: format, args: args);

        /// <summary>
        /// 记录<see cref="Level.Error"/>级别日志
        /// </summary>
        /// <param name="log">日志接口</param>
        /// <param name="provider">字符串格式化提供程序</param>
        /// <param name="format">格式化的字符串</param>
        /// <param name="args">参数集合</param>
        public static ILogging Error(this ILogging log, IFormatProvider provider, string format, params object[] args)
            => log.Write(Level.Error, provider, format, args);

        /// <summary>
        /// 记录<see cref="Level.Error"/>级别日志
        /// </summary>
        /// <param name="log">日志接口</param>
        /// <param name="exception"></param>
        /// <param name="provider">字符串格式化提供程序</param>
        /// <param name="format">格式化的字符串</param>
        /// <param name="args">参数集合</param>
        public static ILogging Error(this ILogging log, Exception exception, IFormatProvider provider, string format, params object[] args)
            => log.Write(Level.Error, exception: exception, provider: provider, format: format, args: args);

        #endregion

        #region Fatal

        /// <summary>
        /// 记录<see cref="Level.Fatal"/>级别日志
        /// </summary>
        /// <param name="log">日志接口</param>
        /// <param name="message">格式化的字符串</param>
        public static ILogging Fatal(this ILogging log, string message)
            => log.Write(Level.Fatal, message);


        /// <summary>
        /// 记录<see cref="Level.Fatal"/>级别日志
        /// </summary>
        /// <param name="log">日志接口</param>
        /// <param name="format">格式化的字符串</param>
        /// <param name="args">参数集合</param>
        public static ILogging Fatal(this ILogging log, string format, params object[] args)
            => log.Write(Level.Fatal, format, args);

        /// <summary>
        /// 记录<see cref="Level.Fatal"/>级别日志
        /// </summary>
        /// <param name="log">日志接口</param>
        /// <param name="provider">字符串格式化提供程序</param>
        /// <param name="format">格式化的字符串</param>
        /// <param name="args">参数集合</param>
        public static ILogging Fatal(this ILogging log, IFormatProvider provider, string format, params object[] args)
            => log.Write(Level.Fatal, provider, format, args);
        #endregion

        #region Trace

        /// <summary>
        /// 记录<see cref="Level.Trace"/>级别日志
        /// </summary>
        /// <param name="log">日志接口</param>
        /// <param name="message">格式化的字符串</param>
        public static ILogging Trace(this ILogging log, string message)
            => log.Write(Level.Trace, message);


        /// <summary>
        /// 记录<see cref="Level.Trace"/>级别日志
        /// </summary>
        /// <param name="log">日志接口</param>
        /// <param name="format">格式化的字符串</param>
        /// <param name="args">参数集合</param>
        public static ILogging Trace(this ILogging log, string format, params object[] args)
            => log.Write(Level.Trace, format, args);

        /// <summary>
        /// 记录<see cref="Level.Trace"/>级别日志
        /// </summary>
        /// <param name="log">日志接口</param>
        /// <param name="provider">字符串格式化提供程序</param>
        /// <param name="format">格式化的字符串</param>
        /// <param name="args">参数集合</param>
        public static ILogging Trace(this ILogging log, IFormatProvider provider, string format, params object[] args)
            => log.Write(Level.Trace, provider, format, args);

        #endregion

        #region Write

        /// <summary>
        /// 
        /// </summary>
        /// <param name="log"></param>
        /// <param name="level"></param>
        /// <param name="exception"></param>
        /// <param name="provider"></param>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static ILogging Write(this ILogging log, Level level, Exception exception, IFormatProvider provider, string format, params object[] args)
        {

            if (format.IsNullOrWhiteSpace() || !log.IsEnabled(level))
                return log;

            var message = format;

            if (args != null && args.Any())
            {
                string[] strArray = new string[args.Length];

                for (var i = 0; i < args.Length; i++)
                {
                    var arg = args[i];
                    if (arg is string)
                    {
                        strArray.SetValue(arg, i);
                    }
                    else
                    {
                        var json = arg.ToJsonString();
                        strArray.SetValue(json.IsNullOrWhiteSpace() ? json : arg.ToString(), i);
                    }
                }

                if (provider == null)
                {
                    message = string.Format(message, strArray);
                }
                else
                {
                    message = string.Format(provider, message, strArray);
                }
            }

            return log.Write(message, level, exception: exception);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="log"></param>
        /// <param name="level"></param>
        /// <param name="provider"></param>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static ILogging Write(this ILogging log, Level level, IFormatProvider provider, string format, params object[] args)
            => log.Write(level: level, exception: null, provider: provider, format: format, args: args);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="log"></param>
        /// <param name="level"></param>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static ILogging Write(this ILogging log, Level level, string format, params object[] args)
            => log.Write(level: level, exception: null, provider: null, format: format, args: args);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="log"></param>
        /// <param name="message"></param>
        /// <param name="level"></param>
        public static ILogging Write(this ILogging log, string message, Level level)
            => log.Write(message: message, level: level, exception: null);
        #endregion


        public static ILogging Create(this ILoggingFactory factory) => factory.Create("Default");
        public static ILogging Create<TCategory>(this ILoggingFactory factory) => factory.Create(typeof(TCategory).Name);
    }
}
