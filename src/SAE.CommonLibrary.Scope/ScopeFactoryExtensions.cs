using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Scope
{
    /// <summary>
    /// ���򹤳���չ
    /// </summary>
    public static class ScopeFactoryExtensions
    {
        /// <summary>
        /// <para><em>ͬ������</em></para>
        /// <para>��õ�ǰ<see cref="IScope"/>����</para>
        /// </summary>
        /// <param name="factory">���򹤳�</param>
        /// <returns>����</returns>
        public static IScope Get(this IScopeFactory factory)
        {
            return factory.GetAsync().GetAwaiter().GetResult();
        }
        /// <summary>
        /// <para><em>ͬ������</em></para>
        /// <para>���ò�����<see cref="IScope"/>����</para>
        /// </summary>
        /// <param name="factory">���򹤳�</param>
        /// <param name="name">��������</param>
        /// <returns>����</returns>
        public static IScope Get(this IScopeFactory factory, string name)
        {
            return factory.GetAsync(name).GetAwaiter().GetResult();
        }
    }
}