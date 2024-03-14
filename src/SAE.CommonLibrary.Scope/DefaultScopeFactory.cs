using System.Reflection.Metadata;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace SAE.CommonLibrary.Scope
{
    /// <summary>
    /// Ĭ�����򹤳�ʵ��
    /// </summary>
    public class DefaultScopeFactory : IScopeFactory
    {
        private readonly AsyncLocal<IScope> _asyncLocal;
        /// <summary>
        /// ���캯��
        /// </summary>
        public DefaultScopeFactory()
        {
            this._asyncLocal = new AsyncLocal<IScope>();
        }

        /// <inheritdoc/>
        public Task<IScope> GetAsync()
        {
            return Task.FromResult<IScope>(this.GetCurrentScope());
        }

        /// <inheritdoc/>
        public Task<IScope> GetAsync(string scopeName)
        {
            var currentScope = this.GetCurrentScope();

            var scope = new DefaultScope(scopeName, currentScope);

            scope.OnDispose += this.Restore;

            this._asyncLocal.Value = scope;

            return Task.FromResult<IScope>(scope);
        }

        /// <summary>
        /// ��õ�ǰ����
        /// </summary>
        /// <returns>�������</returns>
        private IScope GetCurrentScope()
        {
            if (this._asyncLocal.Value == null)
            {
                this._asyncLocal.Value = new DefaultScope(Constants.Default);
            }
            return this._asyncLocal.Value;
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="scope">��������</param>
        private Task Restore(IScope scope)
        {
            this._asyncLocal.Value = scope;
            return Task.CompletedTask;
        }
    }
}