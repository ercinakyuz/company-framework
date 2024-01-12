using Company.Framework.Infrastructure.Application.Context.Builder.Args;

namespace Company.Framework.Domain.Model.Aggregate.Event.Dispatcher
{
    public interface IApplicationContextBuilder
    {
        void Build(ApplicationContextBuilderArgs args);
    }
}