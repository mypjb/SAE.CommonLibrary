using System;

namespace SAE.CommonLibrary.Scope
{
    /// <summary>
    /// scope identity interface
    /// </summary>
    public interface IScope:IDisposable
    {
        /// <summary>
        /// Scope name
        /// </summary>
        string Name { get;}
    }
}