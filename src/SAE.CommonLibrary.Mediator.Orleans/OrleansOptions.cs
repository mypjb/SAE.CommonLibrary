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
        public const string Option = "Orleans";
        public OrleansOptions()
        {
            this.GrainNames = new Dictionary<string, Assembly>();
            this.ClusterId = "dev";
        }
        public string ZooKeeperConnectionString { get; set; }
        public Dictionary<string,Assembly> GrainNames { get;}
        public string ClusterId { get; set; }
    }
}
