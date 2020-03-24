using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary.AspNetCore
{
    public interface IPathDescriptor
    {
        public string Method { get;  }
        public string Path { get;  }
    }

    public class PathDescriptor : IPathDescriptor
    {
        public PathDescriptor()
        {

        }
        public PathDescriptor(string method, string path)
        {
            this.Method = method;
            this.Path = path;
        }
        public string Method { get; set; }
        public string Path { get; set; }
    }
}
