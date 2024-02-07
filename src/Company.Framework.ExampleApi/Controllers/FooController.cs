using Company.Framework.Core.Logging;
using Company.Framework.ExampleApi.Data.Entity;
using Company.Framework.ExampleApi.Data.Repository;
using Company.Framework.ExampleApi.Models.Request;
using Microsoft.AspNetCore.Mvc;

namespace Company.Framework.ExampleApi.Controllers
{
    [ApiController]
    [Route("foo")]
    public class FooController : ControllerBase
    {
        private readonly IActionRepository _actionRepository;

        private readonly IFooRepository _fooRepository;

        public FooController(IActionRepository actionRepository, IFooRepository fooRepository)
        {
            _actionRepository = actionRepository;
            _fooRepository = fooRepository;
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PingActionRequest request)
        {
            var foo = new Foo { State = "Created", Created = Log.Load(request.By), Modified = Log.Load(request.By) };
            await _fooRepository.InsertAsync(foo);
            var action = new ActionEntity { Id = Guid.NewGuid(), State = "Created", Created = Log.Load(request.By), Modified = Log.Load(request.By) };
            await _actionRepository.InsertAsync(action);
            return Created("", new { FooId = foo.Id, ActionId = action.Id });
        }
    }
}
