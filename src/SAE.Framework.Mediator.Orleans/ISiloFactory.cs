using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.Hosting;

namespace SAE.Framework.Mediator.Orleans
{
    public interface ISiloFactory
    {
        IHost Get(Type type);
        IEnumerable<IHost> AsEnumerable();
    }
}
