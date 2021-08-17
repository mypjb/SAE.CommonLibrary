using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAE.CommonLibrary.Mediator.Orleans
{
    public interface IOrleansKeyProvider<TCommand>
    {
        string Get();
    }

    internal class DefaultOrleansKeyProvider<TCommand> : IOrleansKeyProvider<TCommand>
    {
        public string Get()
        {
            var type = typeof(TCommand);
            return type.IsGenericType ?
                   type.GenericTypeArguments.First().Assembly.GetName().Name :
                   type.Assembly.GetName().Name;
        }
    }
}
