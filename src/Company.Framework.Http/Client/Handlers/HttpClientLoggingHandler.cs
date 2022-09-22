using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace Company.Framework.Http.Client.Handlers
{
    public class HttpClientLoggingHandler : DelegatingHandler
    {
        private readonly ILogger<HttpClientLoggingHandler> _logger;

        public HttpClientLoggingHandler(ILogger<HttpClientLoggingHandler> logger)
        {
            _logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            using (_logger.BeginScope(await BuildHttpRequestParametersAsync(request)))
            {
                _logger.LogInformation(
                    $"[HTTP_CLIENT] Request Started {request.Method} {request.RequestUri.OriginalString}");
            }

            var httpResponseMessage = await base.SendAsync(request, cancellationToken);

            using (_logger.BeginScope(await BuildHttpResponseParametersAsync(httpResponseMessage)))
            {
                _logger.LogInformation($"[HTTP_CLIENT] Request Ended {request.Method} {httpResponseMessage.StatusCode}");
            }

            return httpResponseMessage;
        }

        private async Task<Dictionary<string, object>> BuildHttpRequestParametersAsync(HttpRequestMessage httpRequestMessage)
        {
            var httpRequestParameters = new Dictionary<string, object>
            {
                ["Method"] = httpRequestMessage.Method,
                ["Host"] = httpRequestMessage.RequestUri.Host,
                ["Scheme"] = httpRequestMessage.RequestUri.Scheme,
                ["Url"] = httpRequestMessage.RequestUri.OriginalString,
                ["Path"] = httpRequestMessage.RequestUri.AbsolutePath,
                ["Querystring"] = httpRequestMessage.RequestUri.Query,
                ["Body"] = await httpRequestMessage.Content?.ReadAsStringAsync(),
                ["Headers"] = httpRequestMessage.Headers.ToDictionary(x => x.Key, y => JsonSerializer.Serialize(y.Value)),
            };

            return new Dictionary<string, object>
            {
                {"HttpClient",
                    new Dictionary<string,object>
                    {
                        {"Request", httpRequestParameters}
                    }
                }
            };
        }

        private async Task<Dictionary<string, object>> BuildHttpResponseParametersAsync(HttpResponseMessage httpResponseMessage)
        {
            var httpRequestMessage = httpResponseMessage.RequestMessage;
            var httpRequestParameters = new Dictionary<string, object>
            {
                ["Method"] = httpRequestMessage.Method,
                ["Url"] = httpRequestMessage.RequestUri.OriginalString,
                ["Path"] = httpRequestMessage.RequestUri.AbsolutePath,
            };
            var httpResponseParameters = new Dictionary<string, object>
            {
                ["Body"] = await httpResponseMessage.Content.ReadAsStringAsync(),
                ["StatusCode"] = httpResponseMessage.StatusCode,
                ["Headers"] = httpResponseMessage.Headers.ToDictionary(x => x.Key, y => JsonSerializer.Serialize(y.Value)),
            };
            return new Dictionary<string, object>
            {
                {"HttpClient",
                    new Dictionary<string,object>
                    {
                        {"Request", httpRequestParameters},
                        {"Response", httpResponseParameters}
                    }
                }
            };
        }
    }
}