using System;
using System.Collections.Generic;
using System.Text;
using SAE.CommonLibrary.Extension;

namespace SAE.CommonLibrary.AspNetCore.Routing
{
    public interface IPathDescriptor
    {
        /// <summary>
        /// path Index 
        /// Note: the index starts from 1 
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// request method
        /// </summary>
        /// <value></value>
        public string Method { get; }
        /// <summary>
        /// path name
        /// </summary>
        /// <value></value>
        public string Path { get; }
        /// <summary>
        /// display name
        /// </summary>
        /// <value></value>
        public string Name { get; }
        /// <summary>
        /// group name
        /// </summary>
        /// <value></value>
        public string Group { get; }
    }

    public class PathDescriptor : IPathDescriptor
    {
        public PathDescriptor()
        {

        }
        public PathDescriptor(string name, string method, string path, string group)
        {
            this.Name = name?.ToLower().Trim();
            this.Method = method?.ToLower().Trim();
            this.Path = path?.ToLower().Trim();
            this.Group = group?.ToLower().Trim();
            if (this.Name.IsNullOrWhiteSpace())
            {
                this.Name=this.Path;
            }
        }
        public string Method { get; set; }
        public string Path { get; set; }
        public string Name { get; set; }
        public string Group { get; set; }
        public int Index { get; set; }
    }
}
