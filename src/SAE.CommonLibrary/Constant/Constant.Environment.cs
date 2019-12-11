using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary
{
	/// <summary>
	/// 环境变量
	/// </summary>
	public enum Environment
	{
		/// <summary>
		/// 开发环境
		/// </summary>
		Development,
		/// <summary>
		/// 生产环境
		/// </summary>
		Production,
		/// <summary>
		/// 阶段
		/// </summary>
		Stage
	}

	public partial class Constant
	{
		/// <summary>
		/// 环境
		/// </summary>
		public class Environment
		{
			/// <summary>
			/// 环境名称
			/// </summary>
			public static string Name { get; set; } = CommonLibrary.Environment.Development.ToString();
			/// <summary>
			/// 应用名称
			/// </summary>
			public static string ApplicationName = AppContext.TargetFrameworkName ?? "SAE";
		}
	}
}
