using SAE.CommonLibrary.Abstract.Responsibility;
using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary.Configuration.Implement
{
    public class OptionsContext : ResponsibilityContext
    {
        public OptionsContext(string name, Type type) : base()
        {
            this.Name = name;
            this.Type = type;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// 类型
        /// </summary>
        public Type Type { get; }
        private object options;
        /// <summary>
        /// 内容
        /// </summary>
        public object Options
        {
            get => this.options; 
            set
            {
                if (value == null) return;
                this.options = value;
                this.Success();
            }
        }
        /// <summary>
        /// 提供程序
        /// </summary>
        public IOptionsProvider Provider { get; set; }

    }
}
