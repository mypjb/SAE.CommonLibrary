using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SAE.CommonLibrary.Abstract.Decorator;
using SAE.CommonLibrary.Extension;

namespace SAE.CommonLibrary.AspNetCore.Authorization.ABAC
{
    public class HttpABACAuthorizationDescriptorProvider : IABACAuthorizationDescriptorProvider
    {
        public HttpABACAuthorizationDescriptorProvider()
        {
        }

        public Task<IABACAuthorizationDescriptor> GetAsync()
        {
            throw new NotImplementedException();
        }
    }
    
}