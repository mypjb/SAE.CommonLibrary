using System;

namespace SAE.CommonLibrary.ObjectMapper.CodeGenerators.Emitters
{
    internal interface IEmitter
    {
        void Emit(CodeGenerator generator);
    }
}
