using System.Net.Http.Json;
using Company.Framework.Http.Message.Builder;
using Company.Framework.Http.Message.Builder.Args;

namespace Company.Framework.Http.Client
{
    public abstract class CoreHttpClient
    {
        private readonly HttpClient _httpClient;

        private readonly HttpRequestMessageBuilder _httpRequestMessageBuilder;

        protected CoreHttpClient(HttpClient httpClient, HttpRequestMessageBuilder httpRequestMessageBuilder)
        {
            _httpClient = httpClient;
            _httpRequestMessageBuilder = httpRequestMessageBuilder;
        }

        protected async Task<TResponse?> GetAsync<TResponse>(string relativePath, CancellationToken cancellationToken)
        {
            var httpRequestMessage = _httpRequestMessageBuilder.Build(new HttpRequestBuilderArgs(HttpMethod.Get, BuildEndpoint(relativePath)));
            return await SendAsync<TResponse>(httpRequestMessage, cancellationToken);
        }
        protected async Task PostAsync<TRequest>(string relativePath, TRequest request, CancellationToken cancellationToken)
        {
            var httpRequestMessage = _httpRequestMessageBuilder.BuildWithBody(new HttpRequestBuilderArgs<TRequest>(HttpMethod.Post, BuildEndpoint(relativePath), request));
            await SendAsync(httpRequestMessage, cancellationToken);
        }

        protected async Task<TResponse?> PostAsync<TRequest, TResponse>(string relativePath, TRequest request, CancellationToken cancellationToken)
        {
            var httpRequestMessage = _httpRequestMessageBuilder.BuildWithBody(new HttpRequestBuilderArgs<TRequest>(HttpMethod.Post, BuildEndpoint(relativePath), request));
            return await SendAsync<TResponse>(httpRequestMessage, cancellationToken);
        }

        protected async Task PutAsync<TRequest>(string relativePath, TRequest request, CancellationToken cancellationToken)
        {
            var httpRequestMessage = _httpRequestMessageBuilder.BuildWithBody(new HttpRequestBuilderArgs<TRequest>(HttpMethod.Put, BuildEndpoint(relativePath), request));
            await SendAsync(httpRequestMessage, cancellationToken);
        }
        protected async Task<TResponse?> PutAsync<TRequest, TResponse>(string relativePath, TRequest request, CancellationToken cancellationToken)
        {
            var httpRequestMessage = _httpRequestMessageBuilder.BuildWithBody(new HttpRequestBuilderArgs<TRequest>(HttpMethod.Put, BuildEndpoint(relativePath), request));
            return await SendAsync<TResponse>(httpRequestMessage, cancellationToken);
        }
        protected async Task PatchAsync<TRequest>(string relativePath, TRequest request, CancellationToken cancellationToken)
        {
            var httpRequestMessage = _httpRequestMessageBuilder.BuildWithBody(new HttpRequestBuilderArgs<TRequest>(HttpMethod.Patch, BuildEndpoint(relativePath), request));
            await SendAsync(httpRequestMessage, cancellationToken);
        }
        protected async Task<TResponse?> PatchAsync<TRequest, TResponse>(string relativePath, TRequest request, CancellationToken cancellationToken)
        {
            var httpRequestMessage = _httpRequestMessageBuilder.BuildWithBody(new HttpRequestBuilderArgs<TRequest>(HttpMethod.Patch, BuildEndpoint(relativePath), request));
            return await SendAsync<TResponse>(httpRequestMessage, cancellationToken);
        }

        protected async Task DeleteAsync(string relativePath, CancellationToken cancellationToken)
        {
            var httpRequestMessage = _httpRequestMessageBuilder.Build(new HttpRequestBuilderArgs(HttpMethod.Delete, BuildEndpoint(relativePath)));
            await SendAsync(httpRequestMessage, cancellationToken);
        }

        private async Task<TResponse?> SendAsync<TResponse>(HttpRequestMessage httpRequestMessage, CancellationToken cancellationToken, int[]? acceptedHttpStatus = default)
        {
            var response = await SendAsync(httpRequestMessage, cancellationToken);
            return await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken: cancellationToken);
        }

        private async Task<HttpResponseMessage> SendAsync(HttpRequestMessage httpRequestMessage, CancellationToken cancellationToken, int[]? acceptedHttpStatus = default)
        {
            var response = await _httpClient.SendAsync(httpRequestMessage, cancellationToken);
            response.EnsureSuccessStatusCode();
            return response;
        }

        private string BuildEndpoint(string relativePath)
        {
            return $"{_httpClient.BaseAddress}/{relativePath}";
        }
    }
}
