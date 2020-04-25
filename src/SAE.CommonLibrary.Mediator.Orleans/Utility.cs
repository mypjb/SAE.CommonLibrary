using SAE.CommonLibrary.Extension;
using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary.Mediator.Orleans
{
    internal class Utility
    {
        public static string GetIdentity(Type type)
        {
            return type.Assembly.GetName().Name.ToMd5(true);
        }
    }
}
