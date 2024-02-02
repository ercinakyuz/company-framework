using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Company.Framework.ExampleApi.Data.Entity.Configuration
{
    public class ActionEntityConfiguration : IEntityTypeConfiguration<ActionEntity>
    {
        public void Configure(EntityTypeBuilder<ActionEntity> builder)
        {
            builder.ToTable("Action");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.State).HasColumnName("State");
            builder.ComplexProperty(e => e.Created, cpb =>
            {
                cpb.Property(e => e.By).HasColumnName("By").IsRequired();
                cpb.Property(e => e.At).HasColumnName("At").IsRequired();
                cpb.IsRequired();
            });
            builder.ComplexProperty(e => e.Modified, cpb =>
            {
                cpb.Property(e => e.By).HasColumnName("By");
                cpb.Property(e => e.At).HasColumnName("At");
                cpb.IsRequired();
            });
            //builder.ComplexProperty(e => e.Modified);
            //builder.OwnsOne(e => e.Created, onb =>
            //{
            //    onb.Property(e => e.At);
            //    onb.Property(e => e.By);
            //});
            //builder.OwnsOne(e => e.Modified, onb =>
            //{
            //    onb.Property(e => e.At);
            //    onb.Property(e => e.By);
            //});
        }
    }
}
