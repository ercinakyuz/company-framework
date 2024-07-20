namespace Company.Framework.Core.Tenancy.Models
{
    public interface ITenant
    {
        TenantId Id { get; }

        string Name { get; }
    }
}
