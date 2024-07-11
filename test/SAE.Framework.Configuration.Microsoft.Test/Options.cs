using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.Framework.Configuration.Microsoft.Test
{
    public class Options
    {
        public string Data { get; set; }
        public int Version { get; set; }
    }

    public class OfflineOptions: Options
    {
        public bool Init { get; set; }
    }
}
