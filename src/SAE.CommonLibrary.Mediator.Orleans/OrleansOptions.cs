using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;
using Orleans.Hosting;
using Orleans.Configuration;

namespace SAE.CommonLibrary.Mediator.Orleans
{
    public class OrleansOptions
    {
        public OrleansOptions()
        {
            this.GrainNames = new Dictionary<string, Assembly>();
            
        }
        public string ZooKeeperConnectionString { get; set; }
        public Dictionary<string,Assembly> GrainNames { get;}
        public string ClusterId { get; set; }
    }
}
