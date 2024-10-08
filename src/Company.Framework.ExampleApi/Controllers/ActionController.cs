﻿using Company.Framework.ExampleApi.Application.UseCase.GetAllActions.Query;
using Company.Framework.ExampleApi.Application.UseCase.GetAllActions.Stream;
using Company.Framework.ExampleApi.Application.UseCase.Ping.Command;
using Company.Framework.ExampleApi.Application.UseCase.Pong.Command;
using Company.Framework.ExampleApi.Models.Request;
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
        public async Task<IActionResult> Ping([FromBody] PingActionRequest request)
        {
            var id = await _sender.Send(new PingCommand(request.By));
            return Created("", id);
        }

        [HttpPatch]
        [Route("{id}/pong")]
        public async Task<IActionResult> Pong([FromRoute] Guid id, [FromBody] PongActionRequest request)
        {
            await _sender.Send(new PongCommand(id, request.By));
            return Ok();
        }

        [HttpGet]
        [Route("stream")]
        public IAsyncEnumerable<Domain.Model.Aggregate.Action> GetStream()
        {
            return _sender.CreateStream(new GetActionStream());
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var actions = await _sender.Send(new GetActionsQuery());
            return Ok(actions);
        }
    }
}
