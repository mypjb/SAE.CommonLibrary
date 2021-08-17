using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAE.CommonLibrary.Extension;

namespace SAE.CommonLibrary.AspNetCore
{
    /// <summary>
    /// system option
    /// </summary>
    public class SystemOptions
    {
        public SystemOptions()
        {
            this.id = string.Empty;
        }
        public const string Option = "system";
        private string id;
        /// <summary>
        /// system identity
        /// </summary>
        public string Id
        {
            get => this.id; set
            {
                if (!value.IsNullOrWhiteSpace())
                {
                    this.id = value;
                }
            }
        }
    }
}
