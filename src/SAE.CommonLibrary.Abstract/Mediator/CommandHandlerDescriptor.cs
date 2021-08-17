using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary.Abstract.Mediator
{
    public class CommandHandlerDescriptor
    {
        public CommandHandlerDescriptor(Type commandHandlerType,
                                        params Type[] types)
        {
            this.CommandHandlerType = commandHandlerType;
            this.CommandType = types[0];
            this.ResponseType = types.Length == 2 ? types[1] : null;
        }
        public Type CommandHandlerType { get; }
        public Type CommandType { get; }
        public Type ResponseType { get; }
    }
}
