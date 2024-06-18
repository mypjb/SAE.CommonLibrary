using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.Framework.Abstract.Test.Mediator
{
    public class SaveCommand
    {
        public string Name { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
    }

    public class ChangeCommand
    {
        public string Name { get; set; }
    }

    public class RemoveCommand
    {

    }

    public class QueryCommand
    {
        public int Begin { get; set; }
        public int End { get; set; }

        public override string ToString()
        {
            return $"{this.Begin}-{this.End}";
        }
    }

    public class RetryCommand
    {
        public int Number { get; set; }
    }

}
