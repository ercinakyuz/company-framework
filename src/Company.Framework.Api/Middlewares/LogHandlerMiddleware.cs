using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;

namespace Company.Framework.Api.Middlewares
{
    public class LogHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public LogHandlerMiddleware(ILogger<LogHandlerMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            using (_logger.BeginScope(await BuildHttpContextRequestParametersAsync(context.Request)))
            {
                _logger.LogInformation(
                    $"[API] Request Started {context.Request.Method} {context.Request.GetDisplayUrl()}");
            }
            await _next(context);

            using (_logger.BeginScope(await BuildHttpContextResponseParametersAsync(context.Response)))
            {
                _logger.LogInformation(
                    $"[API] Request Ended {context.Request.Method} {context.Request.GetDisplayUrl()} {context.Response.StatusCode}");
            }
        }
        
        private async Task<Dictionary<string, object>> BuildHttpContextRequestParametersAsync(HttpRequest httpRequest)
        {
            var httpContext = httpRequest.HttpContext;

            var httpRequestParameters = new Dictionary<string, object>
            {
                ["IpAddress"] = $"{httpContext.Connection.RemoteIpAddress}",
                ["Host"] = $"{httpRequest.Host}",
                ["Url"] = $"{httpRequest.GetDisplayUrl()}",
                ["Path"] = $"{httpRequest.Path}",
                ["IsHttps"] = httpRequest.IsHttps,
                ["Scheme"] = httpRequest.Scheme,
                ["Method"] = httpRequest.Method,
                ["ContentType"] = httpRequest.ContentType,
                ["Protocol"] = httpRequest.Protocol,
                ["QueryString"] = $"{httpRequest.QueryString}",
                ["Query"] = httpRequest.Query.ToDictionary(x => x.Key, y => y.Value.ToString()),
                ["Headers"] = httpRequest.Headers.ToDictionary(x => x.Key, y => y.Value.ToString()),
                ["Cookies"] = httpRequest.Cookies.ToDictionary(x => x.Key, y => y.Value.ToString()),
                ["Body"] = await BuildRequestBodyAsync(httpRequest)
            };

            return new Dictionary<string, object>
            {
                {"Api",
                    new Dictionary<string,object>
                    {
                        {"Request", httpRequestParameters}
                    }
                }
            };
        }

        private async Task<Dictionary<string, object>> BuildHttpContextResponseParametersAsync(HttpResponse httpResponse)
        {
            var httpContext = httpResponse.HttpContext;
            var httpRequest = httpContext.Request;
            var httpRequestParameters = new Dictionary<string, object>
            {
                ["Url"] = $"{httpRequest.GetDisplayUrl()}",
                ["Path"] = $"{httpRequest.Path}",
                ["Method"] = httpRequest.Method,
            };
            var httpResponseParameters = new Dictionary<string, object>
            {
                ["StatusCode"] = httpResponse.StatusCode,
                ["Body"] = await BuildResponseBodyAsync(httpResponse),
                ["Headers"] = httpResponse.Headers.ToDictionary(x => x.Key, y => y.Value.ToString()),
            };

            return new Dictionary<string, object>
            {
                {"Api",
                    new Dictionary<string,object>
                    {
                        {"Request", httpRequestParameters},
                        {"Response", httpResponseParameters}
                    }
                }
            };
        }

        private async Task<string> BuildRequestBodyAsync(HttpRequest httpRequest)
        {
            httpRequest.EnableBuffering();
            await using var requestStream = new MemoryStream();
            await httpRequest.Body.CopyToAsync(requestStream);
            httpRequest.Body.Position = 0;
            return Encoding.ASCII.GetString(requestStream.ToArray());
        }

        private async Task<string> BuildResponseBodyAsync(HttpResponse httpResponse)
        {
            var originalBodyStream = httpResponse.Body;
            await using var responseBody = new MemoryStream();
            httpResponse.Body = responseBody;
            httpResponse.Body.Seek(0, SeekOrigin.Begin);
            var text = await new StreamReader(httpResponse.Body).ReadToEndAsync();
            httpResponse.Body.Seek(0, SeekOrigin.Begin);
            await responseBody.CopyToAsync(originalBodyStream);
            return text;
        }
    }
}