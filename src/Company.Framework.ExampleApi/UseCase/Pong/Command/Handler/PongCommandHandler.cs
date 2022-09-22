//using Company.Framework.ExampleApi.Http.Clients;
//using MediatR;

//namespace Company.Framework.ExampleApi.UseCase.Pong.Command.Handler;

//public class PongCommandHandler : AsyncRequestHandler<PongCommand>
//{
//    private readonly IActionHttpClient _actionHttpClient;


//    public PongCommandHandler(IActionHttpClient actionHttpClient)
//    {
//        _actionHttpClient = actionHttpClient;
//    }

//    protected override async Task Handle(PongCommand request, CancellationToken cancellationToken)
//    {
//        await _actionHttpClient.Ping(cancellationToken);
//    }
//}