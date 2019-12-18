using SAE.CommonLibrary.Abstract.Responsibility;
using SAE.CommonLibrary.Configuration.Implement;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Configuration
{
    public interface IOptionsProvider : IResponsibility<OptionsContext>
    {
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        Task SaveAsync(string name,object options);
        /// <summary>
        /// 事件更改
        /// </summary>
        /// <returns></returns>
        event Func<Task> OnChange;
    }
}
