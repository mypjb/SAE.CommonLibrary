using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary.Abstract.Responsibility
{
    /// <summary>
    /// IResponsibility Provider
    /// </summary>
    /// <typeparam name="TResponsibilityContext"></typeparam>
    public interface IResponsibilityProvider<TResponsibilityContext> where TResponsibilityContext : ResponsibilityContext
    {
        /// <summary>
        /// Responsibility Main
        /// </summary>
        IResponsibility<TResponsibilityContext> Root { get; }
        /// <summary>
        /// Responsibility List
        /// </summary>
        IEnumerable<IResponsibility<TResponsibilityContext>> Responsibilities { get;  }
    }
}
