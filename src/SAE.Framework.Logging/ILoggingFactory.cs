namespace SAE.Framework.Logging
{
    /// <summary>
    /// 日志工厂
    /// </summary>
    public interface ILoggingFactory
    {
        /// <summary>
        /// 创建具有指定名称的ILog
        /// </summary>
        /// <param name="logName">记录器的名称</param>
        /// <returns>返回一个指定的记录器</returns>
        ILogging Create(string logName);
    }
}