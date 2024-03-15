using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary
{

    public partial class Constants
    {
        /// <summary>
        /// 数据库
        /// </summary>
        public class Database
        {
            /// <summary>
            /// 提供者类型
            /// </summary>
            public class Provider
            {
                /// <summary>
                /// SQLServer
                /// </summary>
                public const string SQLServer = nameof(SQLServer);
                /// <summary>
                /// MySQL
                /// </summary>
                public const string MySQL = nameof(MySQL);
                /// <summary>
                /// OpenGauss
                /// </summary>
                public const string OpenGauss = nameof(OpenGauss);
                /// <summary>
                /// OceanBase
                /// </summary>
                public const string OceanBase = nameof(OceanBase);
            }
        }
    }
}
