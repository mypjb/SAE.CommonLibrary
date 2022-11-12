using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary.Database
{
    /// <summary>
    /// 数据库配置
    /// </summary>
    public class DBConnectOptions
    {
        public const string Option = "database";
        /// <summary>
        /// 链接名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 数据库驱动名称
        /// </summary>
        public string Provider { get; set; }
        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionString { get; set; }
        /// <summary>
        /// 初始化命令（可选）,必须和<see cref="InitialDetectionCommand"/>一起使用
        /// </summary>
        /// <remarks>
        /// <para>
        /// 当程序启动或数据库配置变更时，会使用<see cref="InitialDetectionCommand"/>对数据库发送验证命令，
        /// 如果该命令返回0，则执行初始化命令，否则什么都不做。
        /// </para>
        /// <para>
        /// <em>当设置了<see cref="InitialConnectionString"/>时，会使用该链接执行命令。</em>
        /// </para>
        /// </remarks>
        public string InitialCommand { get; set; }
        /// <summary>
        /// 初始化检测命令（可选），但是必须和<see cref="InitialCommand"/>一起使用。
        /// </summary>
        /// <remarks>
        /// <para>返回0或1</para>
        /// <para><em>当设置了<see cref="InitialConnectionString"/>时，会使用该链接执行命令。</em></para>
        /// </remarks>
        public string InitialDetectionCommand { get; set; }
        /// <summary>
        /// <see cref="InitialCommand"/>和<see cref="InitialDetectionCommand"/>执行时使用的数据库链接,
        /// 如果未设置则使用<see cref="ConnectionString"/>。
        /// </summary>
        public string InitialConnectionString { get; set; }
    }
}
