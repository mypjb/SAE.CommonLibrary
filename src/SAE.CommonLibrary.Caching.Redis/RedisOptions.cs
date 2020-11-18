using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary.Caching.Redis
{
    public class RedisOptions
    {
        public const string Option = "redis";
        public string Connection { get; set; }

        public int DB { get; set; }
    }
}
