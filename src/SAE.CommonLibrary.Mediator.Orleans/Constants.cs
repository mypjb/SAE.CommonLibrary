using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary.Mediator.Orleans
{
    /// <summary>
    /// 常量
    /// </summary>
    internal class Constants
    {
        /// <summary>
        /// 主节点筒仓端口（只支持开发环境）
        /// </summary>
        public const int MasterSiloPort = 11111;
        /// <summary>
        /// 主节点网关端口（只支持开发环境）
        /// </summary>
        public const int MasterGatewayPort = 30000;
    }
}
