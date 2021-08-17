using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SAE.CommonLibrary.Mediator.Orleans
{
    public interface ISiloFactory
    {
        ISiloService Get(Type type);
        IEnumerable<ISiloService> AsEnumerable();
    }
}
