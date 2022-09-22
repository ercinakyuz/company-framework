//using Company.Framework.Http.Client;
//using Company.Framework.Http.Message.Builder;

//namespace Company.Framework.ExampleApi.Http.Clients
//{
//    public class ActionHttpClient : CoreHttpClient, IActionHttpClient
//    {
//        public ActionHttpClient(HttpClient httpClient, HttpRequestMessageBuilder httpRequestMessageBuilder) : base(httpClient, httpRequestMessageBuilder)
//        {

//        }

//        public async Task Ping(CancellationToken cancellationToken)
//        {
//            await PostAsync("ping", new object(), cancellationToken);
//        }
//        public async Task Pong(CancellationToken cancellationToken)
//        {
//            await PostAsync("pong", new object(), cancellationToken);
//        }
//    }
//}
