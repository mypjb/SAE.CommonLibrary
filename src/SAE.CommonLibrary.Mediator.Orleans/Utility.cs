using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary.Mediator.Orleans
{
    internal class Utility
    {
        public static string GetIdentity<TCommand>()
        {
            return typeof(TCommand).Assembly.GetName().Name;
        }
    }
}
