using Company.Framework.Core.Monad;
using Company.Framework.ExampleApi.Data.Entity;
using Company.Framework.ExampleApi.Data.Repository;
using Company.Framework.ExampleApi.Domain.Model.Aggregate.Builder;
using Company.Framework.ExampleApi.Domain.Model.Aggregate.State;
using Company.Framework.ExampleApi.Domain.Model.Aggregate.Value;
using Company.Framework.ExampleApi.Domain.Test.Faker;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using Guid = System.Guid;

namespace Company.Framework.ExampleApi.Domain.Test
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
            _actionRepository = Substitute.For<IActionRepository>();
            _actionBuilder = new ActionBuilder(_actionRepository);
        }

        [Test]
        public async Task Should_Build()
        {
            //Given
            var id = _actionFaker.Id();
            var cancellationToken = CancellationToken.None;
            var entity = _actionFaker.ActionEntity();
            var optionalActionEntity = Optional<ActionEntity>.Of(entity);
            var capturedEntityId = Guid.Empty;
            var entityIdCaptor = Arg.Do<Guid>(entityId => capturedEntityId = entityId);

            //When
            _actionRepository.FindAsync(entityIdCaptor).Returns(optionalActionEntity);
            var result = await _actionBuilder.BuildAsync(id, cancellationToken);

            //Then
            await _actionRepository.Received().FindAsync(Arg.Any<Guid>());

            capturedEntityId.Should().Be(id.Value);

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
}
