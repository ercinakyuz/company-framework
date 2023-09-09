using Company.Framework.Core.Id.Abstractions;
using Company.Framework.Core.Logging;
using Company.Framework.Core.Monad;
using Company.Framework.Domain.Model.Aggregate.State;
using Company.Framework.ExampleApi.Data.Entity;
using Company.Framework.ExampleApi.Data.Repository;
using Company.Framework.ExampleApi.Domain.Model.Aggregate.Builder;
using Company.Framework.ExampleApi.Domain.Model.Aggregate.State;
using Company.Framework.ExampleApi.Domain.Model.Aggregate.Value;
using Company.Framework.Test.Faker;
using FluentAssertions;
using Moq;
using NSubstitute;
using NUnit.Framework;
using Guid = System.Guid;

namespace Company.Framework.ExampleApi.Test
{
    [TestFixture]
    public class ActionBuilderTest
    {
        private static ActionFaker _actionFaker = null!;

        //private Mock<IActionRepository> _actionRepositoryMock = null!;

        private IActionRepository _actionRepository = null!;

        private Guid _capturedIdValue;

        private Guid _idValueArgDo;

        //private CaptureMatch<Guid> _idValueCaptureMatch = null!;

        private ActionBuilder _actionBuilder = null!;



        [OneTimeSetUp]
        public static void BeforeAll()
        {
            _actionFaker = new ActionFaker();
        }

        [SetUp]
        public void BeforeEach()
        {
            //_idValueCaptureMatch = new CaptureMatch<Guid>(value => _capturedIdValue = value);
            _idValueArgDo = Arg.Do<Guid>(value => _capturedIdValue = value);
            //_actionRepositoryMock = new Mock<IActionRepository>();
            _actionRepository = Substitute.For<IActionRepository>();
            _actionBuilder = new ActionBuilder(_actionRepository);
        }

        //[Test]
        //public async Task Should_Build()
        //{
        //    //Given
        //    var id = _actionFaker.Id();
        //    var cancellationToken = CancellationToken.None;
        //    var entity = _actionFaker.ActionEntity();
        //    var optionalAction = Optional<ActionEntity>.Of(entity);

        //    //When
        //    _actionRepositoryMock.Setup(repository => repository.FindAsync(It.IsAny<Guid>())).ReturnsAsync(optionalAction);
        //    var result = await _actionBuilder.BuildAsync(id, cancellationToken);

        //    //Then
        //    _actionRepositoryMock.Verify(repository => repository.FindAsync(Capture.With(_idValueCaptureMatch)), Times.Once);
        //    _actionRepositoryMock.VerifyNoOtherCalls();

        //    _capturedIdValue.Should().Be(id.Value);

        //    result.Should().NotBeNull();
        //    result.Success.Should().BeTrue();
        //    result.Error.Should().BeNull();

        //    var action = result.Data;
        //    action.Should().NotBeNull();
        //    action!.Id.Should().BeEquivalentTo(ActionId.From(entity.Id));
        //    action.State.Should().BeEquivalentTo(ActionState.Loaded);
        //    action.Created.Should().BeEquivalentTo(entity.Created);
        //    action.Modified.Should().BeEquivalentTo(entity.Modified);
        //}

        [Test]
        public async Task Should_Build_N()
        {
            //Given
            var id = _actionFaker.Id();
            var cancellationToken = CancellationToken.None;
            var entity = _actionFaker.ActionEntity();
            var optionalActionEntity = Optional<ActionEntity>.Of(entity);

            //When
            _actionRepository.FindAsync(Arg.Any<Guid>()).ReturnsForAnyArgs(optionalActionEntity);
            var result = await _actionBuilder.BuildAsync(id, cancellationToken);

            //Then
            await _actionRepository.Received().FindAsync(_idValueArgDo);
           // _actionRepository.ReceivedCalls().Should().BeEmpty();

            _capturedIdValue.Should().Be(id.Value);

            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Error.Should().BeNull();

            var action = result.Data;
            action.Should().NotBeNull();
            action!.Id.Should().BeEquivalentTo(ActionId.From(entity.Id));
            action.State.Should().BeEquivalentTo(ActionState.Loaded);
            action.Created.Should().BeEquivalentTo(entity.Created);
            action.Modified.Should().BeEquivalentTo(entity.Modified);
        }
    }

    public class ActionFaker : CoreFaker
    {
        private readonly LogFaker _logFaker;
        private readonly StateFaker<ActionState> _coreStateFaker;

        public ActionFaker()
        {
            _logFaker = new LogFaker();
            _coreStateFaker = StateFaker<ActionState>.Load(ActionState.PingApplied, ActionState.PongApplied);
        }
        public Guid IdValue()
        {
            return Id().Value;
        }

        public ActionId Id()
        {
            return ActionId.New();
        }

        public ActionEntity ActionEntity()
        {
            return new ActionEntity(IdValue(), _coreStateFaker.StateValue(), _logFaker.Log(), _logFaker.Log());
        }

    }

    public class LogFaker : CoreFaker
    {
        public Log Log()
        {
            return Core.Logging.Log.Load(By());
        }

        public string By()
        {
            return Faker.Person.FullName;
        }
    }


    public class StateFaker<TState> : CoreFaker where TState : CoreState<TState>
    {
        private static readonly List<TState> PossibleStates = new()
        {
            CoreState<TState>.Loaded
        };

        private StateFaker() { }

        public static StateFaker<TState> Load(params TState[] states)
        {
            PossibleStates.AddRange(states);
            return new StateFaker<TState>();
        }

        public TState State()
        {
            return Faker.PickRandom(PossibleStates);
        }

        public string StateValue()
        {
            return State().Value;
        }
    }
}
