﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace SAE.CommonLibrary
{
    public class ServiceFacade
    {
        public ServiceFacade(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }
        public static ConcurrentDictionary<string, object> _storge = new ConcurrentDictionary<string, object>();
        public static IServiceProvider ServiceProvider { get; internal set; }

        /// <summary>
        /// 获得<typeparamref name="TService"/>
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <returns></returns>
        public static TService GetService<TService>() where TService : class
        {
            var key = typeof(TService).GUID.ToString();
            return (TService)_storge.GetOrAdd(key, s => ServiceProvider.GetService<TService>());
        }
    }
}
