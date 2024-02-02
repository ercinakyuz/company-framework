using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Company.Framework.ExampleApi.Data.Entity
{
    public class ActionEntityConfiguration : IEntityTypeConfiguration<ActionEntity>
    {
        public void Configure(EntityTypeBuilder<ActionEntity> builder)
        {
            builder.ToTable("Action");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.State);
            builder.OwnsOne(e => e.Created, onb =>
            {
                onb.Property(e => e.At);
                onb.Property(e => e.By);
            });
            builder.OwnsOne(e => e.Modified, onb =>
            {
                onb.Property(e => e.At);
                onb.Property(e => e.By);
            });
        }
    }
}
