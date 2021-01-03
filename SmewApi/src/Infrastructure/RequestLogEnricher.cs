using System;
using System.Linq;
using System.Threading;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Serilog;

namespace Smew.Infrastructure
{
    public static class RequestLogEnricher
    {
        public static void EnrichFromRequest(IDiagnosticContext diagnosticContext, HttpContext httpContext)
        {
            diagnosticContext.Set("ClientIP", httpContext.Connection.RemoteIpAddress.ToString());
            diagnosticContext.Set("UserAgent", httpContext.Request.Headers["User-Agent"].FirstOrDefault());
            diagnosticContext.Set("Resource", httpContext.GetMetricsCurrentResourceName());
        }

        /// <summary>
        /// Gets the current API resource name (specified as Name property of route)
        /// </summary>
        /// <param name="httpContext">The HTTP context</param>
        /// <returns>The current resource name if available, otherwise an empty string</returns>
        public static string GetMetricsCurrentResourceName(this HttpContext httpContext)
        {
            if (httpContext == null)
                throw new ArgumentNullException(nameof(httpContext));

            Endpoint endpoint = httpContext.GetEndpoint();
            return endpoint?.Metadata.GetMetadata<EndpointNameMetadata>()?.EndpointName;
        }
    }
}