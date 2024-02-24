using Microsoft.AspNetCore.SignalR;

namespace Company.Framework.ExampleApi;

public class MyHub : Hub
{
    public override Task OnConnectedAsync()
    {
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        return base.OnDisconnectedAsync(exception);
    }


    public async Task NotifyAll(string message)
    {
        //await Clients.All.SendAsync("PodName", Environment.GetEnvironmentVariable("PodName"));
        //await Clients.All.SendAsync("ChatMessageReceived", chatMessage);
        await Clients.All.SendAsync("greetingResponse", message);
    }
}