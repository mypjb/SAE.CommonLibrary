using System.Threading.Tasks;

namespace SAE.Framework.Scope
{
    /// <summary>
    /// 区域工厂
    /// </summary>
    public interface IScopeFactory
    {
        /// <summary>
        /// 返回当前区域
        /// </summary>
        /// <returns>区域</returns>
        Task<IScope> GetAsync();
        /// <summary> 
        /// 临时设置区域为 <paramref name="scopeName"/>当调用<see cref="System.IDisposable.Dispose"/>接口后将会重置它
        /// </summary>
        /// <param name="scopeName">区域的名称</param>
        /// <returns>区域</returns>
        Task<IScope> GetAsync(string scopeName);
    }
}

