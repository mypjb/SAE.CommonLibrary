using System.Net;
namespace SAE.CommonLibrary.Scope
{
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    internal class DefaultScope : IScope
    {
        /// <summary>
        /// previous scope 
        /// </summary>
        private readonly IScope _previous;
        /// <summary>
        /// scope dispose event
        /// </summary>
        public event Func<IScope, Task> OnDispose;
        /// <summary>
        /// instance default scope
        /// </summary>
        /// <param name="name">scope name</param>
        public DefaultScope(string name)
        {
            this.Name=name;
            this._previous=this;
        }
        /// <summary>
        /// instance new scope
        /// </summary>
        /// <param name="name">new scope name</param>
        /// <param name="previous">Reset to this value when calling <seealso cref="Dispose()"/></param>
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