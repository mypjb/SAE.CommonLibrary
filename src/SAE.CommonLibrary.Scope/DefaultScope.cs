using System;
using System.Net;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Scope
{
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    internal class DefaultScope : IScope
    {
        /// <summary>
        /// ��ǰ������
        /// </summary>
        private readonly IScope _previous;
        /// <summary>
        /// �����ͷŵ��¼�
        /// </summary>
        public event Func<IScope, Task> OnDispose;
        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="name">scope name</param>
        public DefaultScope(string name)
        {
            this.Name=name;
            this._previous=this;
        }
        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="name">�µ���������</param>
        /// <param name="previous">�ͷ�ʱ����Ϊ��ֵ <seealso cref="Dispose()"/></param>
        public DefaultScope(string name, IScope previous)
        {
            this.Name=name;
            this._previous=previous;
        }

        public string Name
        {
            get;
        }

        public void Dispose()
        {
            this.OnDispose?.Invoke(this._previous);
        }
    }
}