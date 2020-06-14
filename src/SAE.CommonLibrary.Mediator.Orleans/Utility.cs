using SAE.CommonLibrary.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SAE.CommonLibrary.Mediator.Orleans
{
    internal class Utility
    {
        public static string GetIdentity(Type type)
        {
            return type.Assembly.GetName().Name;
        }
    }
}
