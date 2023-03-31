using Company.Framework.Domain.Model.Exception;

namespace Company.Framework.ExampleApi.Domain.Model.Aggregate.Builder.Error
{
    public static class ActionBuilderError
    {
        public static DomainError ActionNotFound = new("ACDBE-1", "Action not found");
    }
}
