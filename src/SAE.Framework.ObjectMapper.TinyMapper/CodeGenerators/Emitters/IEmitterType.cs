using System;

namespace SAE.Framework.ObjectMapper.CodeGenerators.Emitters
{
    internal interface IEmitterType : IEmitter
    {
        Type ObjectType { get; }
    }
}
