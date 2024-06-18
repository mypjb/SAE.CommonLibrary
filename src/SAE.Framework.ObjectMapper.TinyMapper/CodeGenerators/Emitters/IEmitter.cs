using System;

namespace SAE.Framework.ObjectMapper.CodeGenerators.Emitters
{
    internal interface IEmitter
    {
        void Emit(CodeGenerator generator);
    }
}
