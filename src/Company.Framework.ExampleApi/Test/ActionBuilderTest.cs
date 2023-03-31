﻿using Company.Framework.Core.Logging;
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
using NUnit.Framework;
using Guid = System.Guid;

namespace Company.Framework.ExampleApi.Test
{
    [TestFixture]
    public class ActionBuilderTest
    {
        private static ActionFaker _actionFaker = null!;

        private Mock<IActionRepository> _actionRepositoryMock = null!;

        private Guid _capturedIdValue;

        private CaptureMatch<Guid> _idValueCaptureMatch = null!;

        private ActionBuilder _actionBuilder = null!;



        [OneTimeSetUp]
        public static void BeforeAll()
        {
            _actionFaker = new ActionFaker();
        }

        [SetUp]
        public void BeforeEach()
        {
            _idValueCaptureMatch = new CaptureMatch<Guid>(value => _capturedIdValue = value);
            _actionRepositoryMock = new Mock<IActionRepository>();
            _actionBuilder = new ActionBuilder(_actionRepositoryMock.Object);
        }

        [Test]
        public async Task Should_Build()
        {
            //Given
            var id = _actionFaker.Id();
            var cancellationToken = CancellationToken.None;
            var entity = _actionFaker.ActionEntity();
            var optionalAction = Optional<ActionEntity>.Of(entity);

            //When
            _actionRepositoryMock.Setup(repository => repository.FindAsync(It.IsAny<Guid>())).ReturnsAsync(optionalAction);
            var result = await _actionBuilder.BuildAsync(id, cancellationToken);

            //Then
            _actionRepositoryMock.Verify(repository => repository.FindAsync(Capture.With(_idValueCaptureMatch)), Times.Once);
            _actionRepositoryMock.VerifyNoOtherCalls();

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
        private readonly CoreStateFaker<ActionState> _coreStateFaker;

        public ActionFaker()
        {
            _logFaker = new LogFaker();
            _coreStateFaker = CoreStateFaker<ActionState>.Load(ActionState.PingApplied, ActionState.PongApplied);
        }
        public Guid IdValue()
        {
            return Id().Value;
        }

        public ActionId Id()
        {
            return ActionId.New;
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


    public class CoreStateFaker<TState> : CoreFaker where TState : CoreState<TState>
    {
        private static readonly List<TState> PossibleStates = new()
        {
            CoreState<TState>.Loaded
        };

        private CoreStateFaker() { }

        public static CoreStateFaker<TState> Load(params TState[] states)
        {
            PossibleStates.AddRange(states);
            return new CoreStateFaker<TState>();
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