using System;
using System.Reflection.Emit;

namespace SAE.CommonLibrary.ObjectMapper.Core
{
    internal static class Helpers
    {
        internal static bool IsValueType(Type type)
        {
            return type.IsValueType;
        }

        internal static bool IsPrimitive(Type type)
        {
            return type.IsPrimitive;
        }

        internal static bool IsEnum(Type type)
        {
            return type.IsEnum;
        }

        internal static bool IsGenericType(Type type)
        {
            return type.IsGenericType;
        }

        internal static Type CreateType(TypeBuilder typeBuilder)
        {
            return typeBuilder.CreateType();
        }

        internal static Type BaseType(Type type)
        {
                return type.BaseType;
        }

    }
}