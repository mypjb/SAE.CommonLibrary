using System;

namespace SAE.CommonLibrary.Scope
{
    /// <summary>
    /// �����ʶ�ӿ�
    /// </summary>
    public interface IScope:IDisposable
    {
        /// <summary>
        /// ���������
        /// </summary>
        string Name { get;}
    }
}