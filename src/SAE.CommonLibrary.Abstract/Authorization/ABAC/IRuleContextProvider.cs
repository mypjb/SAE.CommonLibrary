using System.Collections.Generic;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Abstract.Authorization.ABAC;

/// <summary>
/// <see cref="RuleContext"/>提供者
/// </summary>
public interface IRuleContextProvider
{
    /// <summary>
    /// 
    /// </summary>
    Task<RuleContext> GetAsync();
}
