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

        private readonly IAction2Repository _action2Repository;

        private readonly IAction3Repository _action3Repository;

        private readonly IAction4Repository _action4Repository;

        private readonly IFooRepository _fooRepository;

        public FooController(IActionRepository actionRepository, IFooRepository fooRepository, IAction2Repository action2Repository, IAction3Repository action3Repository, IAction4Repository action4Repository)
        {
            _actionRepository = actionRepository;
            _fooRepository = fooRepository;
            _action2Repository = action2Repository;
            _action3Repository = action3Repository;
            _action4Repository = action4Repository;
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PingActionRequest request)
        {
            var foo = new Foo { State = "Created", Created = Log.Load(request.By), Modified = Log.Load(request.By) };
            var foo2 = new Foo { State = "Created", Created = Log.Load(request.By), Modified = Log.Load(request.By) };
            await _fooRepository.InsertManyAsync(new List<Foo> { foo, foo2 });
            var action = new ActionEntity { Id = Guid.NewGuid(), State = "Created", Created = Log.Load(request.By), Modified = Log.Load(request.By) };
            await _actionRepository.InsertAsync(action);
            await _action2Repository.InsertAsync(action);
            await _action3Repository.InsertAsync(action);
            await _action4Repository.InsertAsync(action);
            return Created("", new { FooId = foo.Id, ActionId = action.Id });
        }
    }
}
