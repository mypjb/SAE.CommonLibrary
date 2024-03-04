using System.Threading.Tasks;

namespace SAE.CommonLibrary.Abstract.Authorization.ABAC;

/// <summary>
/// <see cref="IRuleContextFactory"/>工厂，该实现可以被重写。
/// </summary>
/// <remarks>
/// 默认情况下会组合<see cref="IRuleContextProvider"/>返回的所有<see cref="RuleContext"/>进行返回.
/// </remarks>
public interface IRuleContextFactory
{
    /// <summary>
    /// 返回最终的<see cref="RuleContext"/>对象
    /// </summary>
    /// <returns>规则上下文</returns>
    Task<RuleContext> GetAsync();
}
