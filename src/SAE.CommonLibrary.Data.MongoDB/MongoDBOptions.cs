using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary.Data.MongoDB
{
    public class MongoDBOptions
    {
        internal MongoDBOptions(IServiceCollection serviceCollection)
        {
            this.ServiceCollection = serviceCollection;
        }
       internal IServiceCollection ServiceCollection { get; }
    }
}
