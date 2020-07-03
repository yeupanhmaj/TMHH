using log4net.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPortal.Middleware
{
    public class RequestResponseLoggingMiddleware
    {
        const string MessageTemplateEnd =
             "HTTP path {0} . method {1} . responded {2} in {3} ms";
   
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(RequestResponseLoggingMiddleware));

        readonly RequestDelegate _next;

        public RequestResponseLoggingMiddleware(RequestDelegate next)
        {
            if (next == null) throw new ArgumentNullException(nameof(next));
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext == null) throw new ArgumentNullException(nameof(httpContext));
            if (httpContext.Request.Path.StartsWithSegments(@"/api"))
            {
                var sw = Stopwatch.StartNew();
              
                try
                {
                    await _next(httpContext);
                    sw.Stop();

                    var statusCode = httpContext.Response?.StatusCode;
                    //    var level = statusCode > 499 ? LogEventLevel.Error : LogEventLevel.Information;
                    
   
                    var logContent = String.Format(MessageTemplateEnd, httpContext.Request.Path.Value.ToString(),
                        httpContext.Request.Method, statusCode,
                        sw.Elapsed.TotalMilliseconds);
                    log.Info(logContent);
                }
                // Never caught, because `LogException()` returns false.
                catch (Exception ex) when (LogException(httpContext, sw, ex)) { }
            }
            else
            {
                await _next(httpContext);
            }
        }

        static bool LogException(HttpContext httpContext, Stopwatch sw, Exception ex)
        {
            sw.Stop();
            var statusCode = httpContext.Response?.StatusCode;
            var logContent = String.Format(MessageTemplateEnd, httpContext.Request.Path.Value.ToString(),
                     httpContext.Request.Method, statusCode,
                     sw.Elapsed.TotalMilliseconds);
            log.Info(logContent);

            return false;
        }

      /*  static ILogger LogForErrorContext(HttpContext httpContext)
        {
            var request = httpContext.Request;

            var result = log
                .ForContext("RequestHeaders", request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()), destructureObjects: true)
                .ForContext("RequestHost", request.Host)
                .ForContext("RequestProtocol", request.Protocol);

            if (request.HasFormContentType)
                result = result.ForContext("RequestForm", request.Form.ToDictionary(v => v.Key, v => v.Value.ToString()));

            return result;
        }*/
    }
    public static class RequestResponseLoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestResponseLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestResponseLoggingMiddleware>();
        }
    }
}
