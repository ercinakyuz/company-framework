namespace Company.Framework.Http.Message.Builder.Args;

public record HttpRequestBuilderArgs<TRequest>(HttpMethod HttpMethod, string Endpoint, TRequest RequestBody, IReadOnlyDictionary<string, IEnumerable<string?>>? Headers = default) :
    HttpRequestBuilderArgs(HttpMethod, Endpoint, Headers);

public record HttpRequestBuilderArgs(HttpMethod HttpMethod, string Endpoint,
    IReadOnlyDictionary<string, IEnumerable<string?>>? Headers = default);