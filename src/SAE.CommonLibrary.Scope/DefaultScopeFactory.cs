using System.Reflection.Metadata;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace SAE.CommonLibrary.Scope
{
    /// <summary>
    /// 默认区域工厂实现
    /// </summary>
    /// <inheritdoc/>
    public class DefaultScopeFactory : IScopeFactory
    {
        private readonly AsyncLocal<IScope> _asyncLocal;
        /// <summary>
        /// 构造函数
        /// </summary>
        public DefaultScopeFactory()
        {
            this._asyncLocal = new AsyncLocal<IScope>();
        }

        public Task<IScope> GetAsync()
        {
            return Task.FromResult<IScope>(this.GetCurrentScope());
        }

        public Task<IScope> GetAsync(string scopeName)
        {
            var currentScope = this.GetCurrentScope();

            var scope = new DefaultScope(scopeName, currentScope);

            scope.OnDispose += this.Restore;

            this._asyncLocal.Value = scope;

            return Task.FromResult<IScope>(scope);
        }

        /// <summary>
        /// 获得当前区域
        /// </summary>
        /// <returns></returns>
        private IScope GetCurrentScope()
        {
            if (this._asyncLocal.Value == null)
            {
                this._asyncLocal.Value = new DefaultScope(Constants.Default);
            }
            return this._asyncLocal.Value;
        }

        /// <summary>
        /// 重置区域
        /// </summary>
        /// <param name="scope"></param>
        /// <returns></returns>
        private Task Restore(IScope scope)
        {
            this._asyncLocal.Value = scope;
            return Task.CompletedTask;
        }
    }
}