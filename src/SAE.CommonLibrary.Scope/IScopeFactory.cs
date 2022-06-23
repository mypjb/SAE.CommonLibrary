namespace SAE.CommonLibrary.Scope
{
    /// <summary>
    /// scope factory
    /// </summary>
    public interface IScopeFactory
    {
        /// <summary>
        /// return current scope
        /// </summary>
        /// <returns></returns>
        Task<IScope> GetAsync();
        /// <summary> 
        /// temp setting scope to <paramref name="scopeName"/>Reset to primary when  <seealso cref="IScope.Dispose()"/> is executed
        /// </summary>
        /// <param name="scopeName"></param>
        /// <returns></returns>
        Task<IScope> GetAsync(string scopeName);
    }
}

