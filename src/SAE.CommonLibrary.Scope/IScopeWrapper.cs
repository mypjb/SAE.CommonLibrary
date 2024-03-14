using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Scope
{
    /// <summary>
    /// ��������װ��
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    [Obsolete("this interface is in preview stage")]
    public interface IScopeWrapper<TService> where TService : class
    {
        /// <summary>
        /// ��õ�ǰ����
        /// <param name="key">����key</param>
        /// <param name="constructor"><typeparamref name="TService"/>�������</param>>
        /// </summary>
        TService GetService(string key, Func<TService> constructor);

        /// <summary>
        /// ����
        /// </summary>
        void Clear();
    }
}