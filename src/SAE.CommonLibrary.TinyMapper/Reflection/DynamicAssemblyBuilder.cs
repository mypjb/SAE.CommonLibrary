using System;
using System.Reflection;
using System.Reflection.Emit;

namespace SAE.CommonLibrary.ObjectMapper.Reflection
{
    internal class DynamicAssemblyBuilder
    {
        internal const string AssemblyName = "DynamicTinyMapper";
#if !COREFX
//        private const string AssemblyNameFileName = AssemblyName + ".dll";
//        private static AssemblyBuilder _assemblyBuilder;
#endif
        private static readonly DynamicAssembly _dynamicAssembly = new DynamicAssembly();

        public static IDynamicAssembly Get()
        {
            return _dynamicAssembly;
        }


        private sealed class DynamicAssembly : IDynamicAssembly
        {
            private readonly ModuleBuilder _moduleBuilder;

            public DynamicAssembly()
            {
                var assemblyName = new AssemblyName(AssemblyName);

                AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
                _moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name);


            }

            public TypeBuilder DefineType(string typeName, Type parentType)
            {
                return _moduleBuilder.DefineType(typeName, TypeAttributes.Public | TypeAttributes.Sealed, parentType, null);
            }

            public void Save()
            {
            }
        }
    }
}
