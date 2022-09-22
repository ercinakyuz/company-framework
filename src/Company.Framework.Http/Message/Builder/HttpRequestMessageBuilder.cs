using System.Net.Http.Headers;
using System.Text;
using Company.Framework.Http.Message.Builder.Args;
using Newtonsoft.Json;

namespace Company.Framework.Http.Message.Builder
{
    public class HttpRequestMessageBuilder
    {
        public HttpRequestMessage Build(HttpRequestBuilderArgs args)
        {

            var httpRequestMessage = new HttpRequestMessage(args.HttpMethod, args.Endpoint);
            httpRequestMessage.Headers.Accept.Clear();
            httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (args.Headers != default)
                foreach (var header in args.Headers)
                    httpRequestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value);
            return httpRequestMessage;
        }

        public HttpRequestMessage BuildWithBody<TRequest>(HttpRequestBuilderArgs<TRequest> args)
        {
            var httpRequestMessage = Build(args);
            httpRequestMessage.Content = BuildStringContent(args.RequestBody);
            return httpRequestMessage;
        }

        private static StringContent BuildStringContent<TRequest>(TRequest request)
        {
            return new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
        }
    }
}
