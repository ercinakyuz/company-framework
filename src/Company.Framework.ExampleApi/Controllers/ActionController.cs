using Company.Framework.ExampleApi.Domain.Model.Aggregate.Value;
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
            var id = await _sender.Send(new PingCommand());
            return Created("", id);
        }

        [HttpPatch]
        [Route("pong/{id}")]
        public async Task<IActionResult> Pong(Guid id)
        {
            await _sender.Send(new PongCommand(ActionId.From(id)));
            return Ok();
        }
    }
}
