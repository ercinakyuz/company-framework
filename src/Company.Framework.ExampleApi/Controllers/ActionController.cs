using Company.Framework.ExampleApi.UseCase.Ping.Command;
using Company.Framework.ExampleApi.UseCase.Pong.Command;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Company.Framework.ExampleApi.Controllers
{
    [ApiController]
    [Route("action")]
    public class ActionController : ControllerBase
    {
        private readonly ISender _sender;

        public ActionController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost]
        [Route("ping")]
        public async Task<IActionResult> Ping()
        {
            await _sender.Send(new PingCommand());
            return Ok();
        }

        [HttpPost]
        [Route("pong")]
        public async Task<IActionResult> Pong()
        {
            await _sender.Send(new PongCommand());
            return Ok();
        }
    }
}
