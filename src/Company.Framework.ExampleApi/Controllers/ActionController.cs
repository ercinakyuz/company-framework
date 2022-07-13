using Company.Framework.ExampleApi.UseCase.Ping.Command;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Company.Framework.ExampleApi.Controllers
{
    [Route("action")]
    public class ActionController : Controller
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
    }
}
