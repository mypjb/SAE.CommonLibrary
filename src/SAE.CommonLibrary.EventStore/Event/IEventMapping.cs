using SAE.CommonLibrary.Extension;
using SAE.CommonLibrary.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary.EventStore
{
    /// <summary>
    /// event mapping provider
    /// </summary>
    public interface IEventMapping
    {
        /// <summary>
        /// add mapping
        /// </summary>
        /// <param name="key">mapping key</param>
        /// <param name="type">mapping type</param>
        void Add(string key, Type type);
        /// <summary>
        /// get mapping
        /// </summary>
        /// <param name="key">mapping key</param>
        /// <returns>return mapping type</returns>
        Type Get(string key);
    }

    /// <summary>
    /// default <seealso cref="IEventMapping"/> implementation
    /// </summary>
    public class DefaultEventMapping : IEventMapping
    {
        private readonly ILogging<DefaultEventMapping> _logging;
        private readonly Dictionary<string, Type> _mapping;
        public DefaultEventMapping(ILogging<DefaultEventMapping> logging,IEnumerable<EventMappingProvider> providers)
        {
            this._logging = logging;
            this._mapping = new Dictionary<string, Type>();
            foreach (var provider in providers)
            {
                foreach (var type in provider.Types)
                {
                    this.Add(type.GetIdentity(), type);
                }
            }
        }
        public void Add(string key, Type type)
        {
            this._mapping[key] = type;
            this._logging.Info($"add mapping: key:'{key}',type:{type}");
        }

        public Type Get(string key)
        {
            Type type;

            if (!this._mapping.TryGetValue(key, out type))
            {
                var message = $"not exist '{key}' the mapping";
                this._logging.Error(message);
                throw new SaeException(StatusCodes.ResourcesExist, message);
            }

            return type;
        }
    }
}
