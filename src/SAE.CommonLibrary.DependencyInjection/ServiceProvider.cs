using System;
using System.Collections.Generic;
using System.Text;

namespace SAE.CommonLibrary.DependencyInjection
{
    public static class ServiceProvider
    {
        public static IServiceProvider Current { get; internal set; }
    }
}
