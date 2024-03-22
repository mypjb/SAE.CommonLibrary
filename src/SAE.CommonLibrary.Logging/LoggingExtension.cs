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
        /// <param name="logging">日志接口</param>
        /// <param name="message">格式化的字符串</param>
        /// <returns><paramref name="logging"/></returns>
        public static ILogging Debug(this ILogging logging, string message)
            => logging.Write(Level.Debug, message);


        /// <summary>
        /// 记录<see cref="Level.Debug"/>级别日志
        /// </summary>
        /// <param name="logging">日志接口</param>
        /// <param name="format">格式化的字符串</param>
        /// <param name="args">参数集合</param>
        /// <returns><paramref name="logging"/></returns>
        public static ILogging Debug(this ILogging logging, string format, params object[] args)
            => logging.Write(Level.Debug, format, args);

        /// <summary>
        /// 记录<see cref="Level.Debug"/>级别日志
        /// </summary>
        /// <param name="logging">日志接口</param>
        /// <param name="provider">字符串格式化提供程序</param>
        /// <param name="format">格式化的字符串</param>
        /// <param name="args">参数集合</param>
        /// <returns><paramref name="logging"/></returns>
        public static ILogging Debug(this ILogging logging, IFormatProvider provider, string format, params object[] args)
            => logging.Write(Level.Debug, provider, format, args);
        #endregion

        #region Info

        /// <summary>
        /// 记录<see cref="Level.Info"/>级别日志
        /// </summary>
        /// <param name="logging">日志接口</param>
        /// <param name="message">格式化的字符串</param>
        /// <returns><paramref name="logging"/></returns>
        public static ILogging Info(this ILogging logging, string message)
            => logging.Write(Level.Info, message);


        /// <summary>
        /// 记录<see cref="Level.Info"/>级别日志
        /// </summary>
        /// <param name="logging">日志接口</param>
        /// <param name="format">格式化的字符串</param>
        /// <param name="args">参数集合</param>
        /// <returns><paramref name="logging"/></returns>
        public static ILogging Info(this ILogging logging, string format, params object[] args)
            => logging.Write(Level.Info, format, args);

        /// <summary>
        /// 记录<see cref="Level.Info"/>级别日志
        /// </summary>
        /// <param name="logging">日志接口</param>
        /// <param name="provider">字符串格式化提供程序</param>
        /// <param name="format">格式化的字符串</param>
        /// <param name="args">参数集合</param>
        /// <returns><paramref name="logging"/></returns>
        public static ILogging Info(this ILogging logging, IFormatProvider provider, string format, params object[] args)
            => logging.Write(Level.Info, provider, format, args);

        #endregion

        #region Warn
        /// <summary>
        /// 记录<see cref="Level.Warn"/>级别日志
        /// </summary>
        /// <param name="logging">日志接口</param>
        /// <param name="message">格式化的字符串</param>
        /// <returns><paramref name="logging"/></returns>
        public static ILogging Warn(this ILogging logging, string message)
            => logging.Write(Level.Warn, message);


        /// <summary>
        /// 记录<see cref="Level.Warn"/>级别日志
        /// </summary>
        /// <param name="logging">日志接口</param>
        /// <param name="format">格式化的字符串</param>
        /// <param name="args">参数集合</param>
        /// <returns><paramref name="logging"/></returns>
        public static ILogging Warn(this ILogging logging, string format, params object[] args)
            => logging.Write(Level.Warn, format, args);

        /// <summary>
        /// 记录<see cref="Level.Warn"/>级别日志
        /// </summary>
        /// <param name="logging">日志接口</param>
        /// <param name="provider">字符串格式化提供程序</param>
        /// <param name="format">格式化的字符串</param>
        /// <param name="args">参数集合</param>
        /// <returns><paramref name="logging"/></returns>
        public static ILogging Warn(this ILogging logging, IFormatProvider provider, string format, params object[] args)
            => logging.Write(Level.Warn, provider, format, args);
        #endregion

        #region Error

        /// <summary>
        /// 记录<see cref="Level.Error"/>级别日志
        /// </summary>
        /// <param name="logging">日志接口</param>
        /// <param name="exception">异常</param>
        /// <returns><paramref name="logging"/></returns>
        public static ILogging Error(this ILogging logging, Exception exception)
            => logging.Write(Level.Error, exception, null, exception?.Message);

        /// <summary>
        /// 记录<see cref="Level.Error"/>级别日志
        /// </summary>
        /// <param name="logging">日志接口</param>
        /// <param name="message">格式化的字符串</param>
        /// <returns><paramref name="logging"/></returns>
        public static ILogging Error(this ILogging logging, string message)
            => logging.Write(Level.Error, message);

        /// <summary>
        /// 记录<see cref="Level.Error"/>级别日志
        /// </summary>
        /// <param name="logging">日志接口</param>
        /// <param name="exception"></param>
        /// <param name="message">格式化的字符串</param>
        /// <returns><paramref name="logging"/></returns>
        public static ILogging Error(this ILogging logging, Exception exception, string message)
            => logging.Write(Level.Error, exception, null, message);

        /// <summary>
        /// 记录<see cref="Level.Error"/>级别日志
        /// </summary>
        /// <param name="logging">日志接口</param>
        /// <param name="format">格式化的字符串</param>
        /// <param name="args">参数集合</param>
        /// <returns><paramref name="logging"/></returns>
        public static ILogging Error(this ILogging logging, string format, params object[] args)
            => logging.Write(Level.Error, format, args);

        /// <summary>
        /// 记录<see cref="Level.Error"/>级别日志
        /// </summary>
        /// <param name="logging">日志接口</param>
        /// <param name="exception"></param>
        /// <param name="format">格式化的字符串</param>
        /// <param name="args">参数集合</param>
        /// <returns><paramref name="logging"/></returns>
        public static ILogging Error(this ILogging logging, Exception exception, string format, params object[] args)
            => logging.Write(Level.Error, exception: exception, provider: null, format: format, args: args);

        /// <summary>
        /// 记录<see cref="Level.Error"/>级别日志
        /// </summary>
        /// <param name="logging">日志接口</param>
        /// <param name="provider">字符串格式化提供程序</param>
        /// <param name="format">格式化的字符串</param>
        /// <param name="args">参数集合</param>
        /// <returns><paramref name="logging"/></returns>
        public static ILogging Error(this ILogging logging, IFormatProvider provider, string format, params object[] args)
            => logging.Write(Level.Error, provider, format, args);

        /// <summary>
        /// 记录<see cref="Level.Error"/>级别日志
        /// </summary>
        /// <param name="logging">日志接口</param>
        /// <param name="exception"></param>
        /// <param name="provider">字符串格式化提供程序</param>
        /// <param name="format">格式化的字符串</param>
        /// <param name="args">参数集合</param>
        /// <returns><paramref name="logging"/></returns>
        public static ILogging Error(this ILogging logging, Exception exception, IFormatProvider provider, string format, params object[] args)
            => logging.Write(Level.Error, exception: exception, provider: provider, format: format, args: args);

        #endregion

        #region Fatal

        /// <summary>
        /// 记录<see cref="Level.Fatal"/>级别日志
        /// </summary>
        /// <param name="logging">日志接口</param>
        /// <param name="message">格式化的字符串</param>
        /// <returns><paramref name="logging"/></returns>
        public static ILogging Fatal(this ILogging logging, string message)
            => logging.Write(Level.Fatal, message);


        /// <summary>
        /// 记录<see cref="Level.Fatal"/>级别日志
        /// </summary>
        /// <param name="logging">日志接口</param>
        /// <param name="format">格式化的字符串</param>
        /// <param name="args">参数集合</param>
        /// <returns><paramref name="logging"/></returns>
        public static ILogging Fatal(this ILogging logging, string format, params object[] args)
            => logging.Write(Level.Fatal, format, args);

        /// <summary>
        /// 记录<see cref="Level.Fatal"/>级别日志
        /// </summary>
        /// <param name="logging">日志接口</param>
        /// <param name="provider">字符串格式化提供程序</param>
        /// <param name="format">格式化的字符串</param>
        /// <param name="args">参数集合</param>
        /// <returns><paramref name="logging"/></returns>
        public static ILogging Fatal(this ILogging logging, IFormatProvider provider, string format, params object[] args)
            => logging.Write(Level.Fatal, provider, format, args);
        #endregion

        #region Trace

        /// <summary>
        /// 记录<see cref="Level.Trace"/>级别日志
        /// </summary>
        /// <param name="logging">日志接口</param>
        /// <param name="message">格式化的字符串</param>
        /// <returns><paramref name="logging"/></returns>
        public static ILogging Trace(this ILogging logging, string message)
            => logging.Write(Level.Trace, message);


        /// <summary>
        /// 记录<see cref="Level.Trace"/>级别日志
        /// </summary>
        /// <param name="logging">日志接口</param>
        /// <param name="format">格式化的字符串</param>
        /// <param name="args">参数集合</param>
        /// <returns><paramref name="logging"/></returns>
        public static ILogging Trace(this ILogging logging, string format, params object[] args)
            => logging.Write(Level.Trace, format, args);

        /// <summary>
        /// 记录<see cref="Level.Trace"/>级别日志
        /// </summary>
        /// <param name="logging">日志接口</param>
        /// <param name="provider">字符串格式化提供程序</param>
        /// <param name="format">格式化的字符串</param>
        /// <param name="args">参数集合</param>
        /// <returns><paramref name="logging"/></returns>
        public static ILogging Trace(this ILogging logging, IFormatProvider provider, string format, params object[] args)
            => logging.Write(Level.Trace, provider, format, args);

        #endregion

        #region Write

        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="logging">日志记录器</param>
        /// <param name="level">日志级别</param>
        /// <param name="exception">异常信息</param>
        /// <param name="provider">日志信息格式化器</param>
        /// <param name="format">待格式化字符串</param>
        /// <param name="args">参数</param>
        /// <returns><paramref name="logging"/></returns>
        public static ILogging Write(this ILogging logging, Level level, Exception exception, IFormatProvider provider, string format, params object[] args)
        {

            if (format.IsNullOrWhiteSpace() || !logging.IsEnabled(level))
                return logging;

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

            return logging.Write(message, level, exception: exception);
        }
        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="logging">日志记录器</param>
        /// <param name="level">日志级别</param>
        /// <param name="provider">日志信息格式化器</param>
        /// <param name="format">待格式化字符串</param>
        /// <param name="args">参数</param>
        /// <returns><paramref name="logging"/></returns>
        public static ILogging Write(this ILogging logging, Level level, IFormatProvider provider, string format, params object[] args)
            => logging.Write(level: level, exception: null, provider: provider, format: format, args: args);
        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="logging">日志记录器</param>
        /// <param name="level">日志级别</param>
        /// <param name="format">待格式化字符串</param>
        /// <param name="args">参数</param>
        /// <returns><paramref name="logging"/></returns>
        public static ILogging Write(this ILogging logging, Level level, string format, params object[] args)
            => logging.Write(level: level, exception: null, provider: null, format: format, args: args);
        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="logging">日志记录器</param>
        /// <param name="message">写入信息</param>
        /// <param name="level">日志级别</param>
        /// <returns><paramref name="logging"/></returns>
        public static ILogging Write(this ILogging logging, string message, Level level)
            => logging.Write(level, null,null, message);
        #endregion
        /// <summary>
        /// 创建日志记录器
        /// </summary>
        /// <param name="factory">日志工厂</param>
        /// <returns>日志记录器</returns>
        public static ILogging Create(this ILoggingFactory factory) => factory.Create("Default");
        /// <summary>
        /// 创建日志记录器
        /// </summary>
        /// <param name="factory">日志工厂</param>
        /// <typeparam name="TCategory">日志类型</typeparam>
        /// <returns><see cref="ILogging"/></returns>
        public static ILogging Create<TCategory>(this ILoggingFactory factory) => factory.Create(typeof(TCategory).Name);
    }
}
