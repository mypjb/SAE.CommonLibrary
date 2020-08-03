using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary.ObjectMapper
{
    public interface IObjectMapper
    {
        IObjectMapper Bind(Type sourceType, Type targetType);
        bool BindingExists(Type sourceType, Type targetType);
        object Map(Type sourceType, Type targetType, object source, object target = null);
    }
}
