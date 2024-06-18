using System.Collections.Generic;
using System.Threading.Tasks;

namespace SAE.Framework.Abstract.Authorization.ABAC;

/// <summary>
/// <see cref="RuleContext"/>提供者
/// </summary>
public interface IRuleContextProvider
{
    
    /// <summary>
    /// 获得当前规则上下文
    /// </summary>
    /// <returns>规则上下文</returns>
    Task<RuleContext> GetAsync();
}
