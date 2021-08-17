using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary.Data
{
    public class StorageOptions
    {
        internal IServiceCollection ServiceCollection { get; }

        public StorageOptions(IServiceCollection serviceCollection)
        {
            this.ServiceCollection = serviceCollection;
        }
    }
}
