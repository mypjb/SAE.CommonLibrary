using SAE.Framework.Extension;
using System;
using System.Linq;

namespace SAE.Framework.Logging
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
        /// <param name="message">日志信息</param>
        /// <returns><paramref name="logging"/></returns>
        public static ILogging Debug(this ILogging logging, string message)
            => logging.Write(Level.Debug, message);

        /// <summary>
        /// 传入一个委托，当<see cref="Level.Debug"/>被启用时，会调用委托获取日志信息。
        /// </summary>
        /// <remarks>
        /// 此函数是为了解决<em>日志信息</em>的生成需要占用硬件大量资源。
        /// </remarks>
        /// <param name="logging">日志接口</param>
        /// <param name="messageDelegate">消息委托</param>
        /// <returns><paramref name="logging"/></returns>
        public static ILogging Debug(this ILogging logging, Func<string> messageDelegate)
        {
            if (logging.IsEnabled(Level.Debug))
            {
                logging.Debug(messageDelegate.Invoke());
            }

            return logging;
        }

        #endregion

        #region Info

        /// <summary>
        /// 记录<see cref="Level.Info"/>级别日志
        /// </summary>
        /// <param name="logging">日志接口</param>
        /// <param name="message">日志信息</param>
        /// <returns><paramref name="logging"/></returns>
        public static ILogging Info(this ILogging logging, string message)
            => logging.Write(Level.Info, message);

        /// <summary>
        /// 传入一个委托，当<see cref="Level.Info"/>被启用时，会调用委托获取日志信息。
        /// </summary>
        /// <remarks>
        /// 此函数是为了解决<em>日志信息</em>的生成需要占用硬件大量资源。
        /// </remarks>
        /// <param name="logging">日志接口</param>
        /// <param name="messageDelegate">消息委托</param>
        /// <returns><paramref name="logging"/></returns>
        public static ILogging Info(this ILogging logging, Func<string> messageDelegate)
        {
            if (logging.IsEnabled(Level.Info))
            {
                logging.Info(messageDelegate.Invoke());
            }

            return logging;
        }

        #endregion

        #region Warn
        /// <summary>
        /// 记录<see cref="Level.Warn"/>级别日志
        /// </summary>
        /// <param name="logging">日志接口</param>
        /// <param name="message">日志信息</param>
        /// <returns><paramref name="logging"/></returns>
        public static ILogging Warn(this ILogging logging, string message)
            => logging.Write(Level.Warn, message);

        /// <summary>
        /// 传入一个委托，当<see cref="Level.Warn"/>被启用时，会调用委托获取日志信息。
        /// </summary>
        /// <remarks>
        /// 此函数是为了解决<em>日志信息</em>的生成需要占用硬件大量资源。
        /// </remarks>
        /// <param name="logging">日志接口</param>
        /// <param name="messageDelegate">消息委托</param>
        /// <returns><paramref name="logging"/></returns>
        public static ILogging Warn(this ILogging logging, Func<string> messageDelegate)
        {
            if (logging.IsEnabled(Level.Warn))
            {
                logging.Warn(messageDelegate.Invoke());
            }

            return logging;
        }


        #endregion

        #region Error

        /// <summary>
        /// 记录<see cref="Level.Error"/>级别日志
        /// </summary>
        /// <param name="logging">日志接口</param>
        /// <param name="exception">异常</param>
        /// <returns><paramref name="logging"/></returns>
        public static ILogging Error(this ILogging logging, Exception exception)
            => logging.Write(Level.Error, exception?.Message, exception);

        /// <summary>
        /// 传入一个委托，当<see cref="Level.Error"/>被启用时，会调用委托获取日志信息。
        /// </summary>
        /// <remarks>
        /// 此函数是为了解决<em>日志信息</em>的生成需要占用硬件大量资源。
        /// </remarks>
        /// <param name="logging">日志接口</param>
        /// <param name="messageDelegate">消息委托</param>
        /// <returns><paramref name="logging"/></returns>
        public static ILogging Error(this ILogging logging, Func<string> messageDelegate)
        {
            if (logging.IsEnabled(Level.Error))
            {
                logging.Error(messageDelegate.Invoke());
            }

            return logging;
        }

        /// <summary>
        /// 传入一个委托，当<see cref="Level.Error"/>被启用时，会调用委托获取日志信息。
        /// </summary>
        /// <remarks>
        /// 此函数是为了解决<em>日志信息</em>的生成需要占用硬件大量资源。
        /// </remarks>
        /// <param name="logging">日志接口</param>
        /// <param name="messageDelegate">消息委托</param>
        /// <param name="exception">触发的异常</param>
        /// <returns><paramref name="logging"/></returns>
        public static ILogging Error(this ILogging logging, Func<string> messageDelegate, Exception exception)
        {
            if (logging.IsEnabled(Level.Error))
            {
                logging.Error(messageDelegate.Invoke(), exception);
            }

            return logging;
        }

        /// <summary>
        /// 记录<see cref="Level.Error"/>级别日志
        /// </summary>
        /// <param name="logging">日志接口</param>
        /// <param name="message">日志信息</param>
        /// <returns><paramref name="logging"/></returns>
        public static ILogging Error(this ILogging logging, string message)
            => logging.Write(Level.Error, message);

        /// <summary>
        /// 记录<see cref="Level.Error"/>级别日志
        /// </summary>
        /// <param name="logging">日志接口</param>
        /// <param name="exception"></param>
        /// <param name="message">日志信息</param>
        /// <returns><paramref name="logging"/></returns>
        public static ILogging Error(this ILogging logging, string message, Exception exception)
            => logging.Write(Level.Error, message, exception);


        #endregion

        #region Fatal

        /// <summary>
        /// 记录<see cref="Level.Fatal"/>级别日志
        /// </summary>
        /// <param name="logging">日志接口</param>
        /// <param name="message">日志信息</param>
        /// <returns><paramref name="logging"/></returns>
        public static ILogging Fatal(this ILogging logging, string message)
            => logging.Write(Level.Fatal, message);

        #endregion

        #region Trace

        /// <summary>
        /// 记录<see cref="Level.Trace"/>级别日志
        /// </summary>
        /// <param name="logging">日志接口</param>
        /// <param name="message">日志信息</param>
        /// <returns><paramref name="logging"/></returns>
        public static ILogging Trace(this ILogging logging, string message)
            => logging.Write(Level.Trace, message);

        /// <summary>
        /// 传入一个委托，当<see cref="Level.Trace"/>被启用时，会调用委托获取日志信息。
        /// </summary>
        /// <remarks>
        /// 此函数是为了解决<em>日志信息</em>的生成需要占用硬件大量资源。
        /// </remarks>
        /// <param name="logging">日志接口</param>
        /// <param name="messageDelegate">消息委托</param>
        /// <returns><paramref name="logging"/></returns>
        public static ILogging Trace(this ILogging logging, Func<string> messageDelegate)
        {
            if (logging.IsEnabled(Level.Trace))
            {
                logging.Trace(messageDelegate.Invoke());
            }

            return logging;
        }


        #endregion

        #region Write

        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="logging">日志记录器</param>
        /// <param name="level">日志级别</param>
        /// <param name="exception">异常信息</param>
        /// <param name="message">待格式化字符串</param>
        /// <returns><paramref name="logging"/></returns>
        public static ILogging Write(this ILogging logging, Level level, string message, Exception exception)
        {

            if (message.IsNullOrWhiteSpace() || !logging.IsEnabled(level))
                return logging;

            return logging.Write(message, level, exception: exception);
        }
        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="logging">日志记录器</param>
        /// <param name="level">日志级别</param>
        /// <param name="message">待格式化字符串</param>
        /// <returns><paramref name="logging"/></returns>
        public static ILogging Write(this ILogging logging, Level level, string message)
            => logging.Write(level, message, null);

        #endregion
        /// <summary>
        /// 创建日志记录器
        /// </summary>
        /// <param name="factory">日志工厂</param>
        /// <returns><see cref="ILogging"/></returns>
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
