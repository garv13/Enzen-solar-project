using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IOTSolar
{
    public class TraceHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Debug.WriteLine("MOBILE SERVICE REQUEST:");
            Debug.WriteLine("Uri: " + request.RequestUri);
            Debug.WriteLine("Method: " + request.Method);

            if (request.Content != null)
            {
                var content = await request.Content.ReadAsStringAsync();
                Debug.WriteLineIf(!string.IsNullOrWhiteSpace(content),
                    "Content: " + content);
            }

            Debug.WriteLine("======");

            // Sends the actual request to the Mobile Service.
            var response = await base.SendAsync(request, cancellationToken);
            return response;
        }
    }
}
