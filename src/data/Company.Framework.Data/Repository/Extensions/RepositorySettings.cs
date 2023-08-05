namespace Company.Framework.Data.Repository.Extensions
{
    public record RepositorySettings(string Instance, string Context, string? TableOrCollection = default);
}