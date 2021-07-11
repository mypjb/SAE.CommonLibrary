using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SAE.CommonLibrary.Extension.Middleware
{
    public class ExceptionMiddleware : DelegatingHandler
    {
        private readonly Func<HttpResponseMessage, Task> _handler;

        public ExceptionMiddleware(Func<HttpResponseMessage, Task> handler)
        {
            this._handler = handler;
        }

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var responseMessage = await base.SendAsync(request, cancellationToken); ;

            if (!responseMessage.IsSuccessStatusCode)
            {
                await this._handler.Invoke(responseMessage);
            }
            return responseMessage;
        }
    }
}

