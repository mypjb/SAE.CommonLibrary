using System;
using System.Reflection.Emit;

namespace SAE.Framework.ObjectMapper.Reflection
{
    internal interface IDynamicAssembly
    {
        TypeBuilder DefineType(string typeName, Type parentType);
        void Save();
    }
}
