using FinancialBudget.Server.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinancialBudget.Server.Context.Config
{
    public class OriginConfig : IEntityTypeConfiguration<Origin>
    {
        public void Configure(EntityTypeBuilder<Origin> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasData(
                new Origin { Id = 1, Name = "Mantenimiento", State = 1, CreatedBy = 1, CreatedAt = new DateTimeOffset(2025,10,1,0,0,0,TimeSpan.Zero) },
                new Origin { Id = 2, Name = "Eventos", State = 1, CreatedBy = 1, CreatedAt = new DateTimeOffset(2025,10,1,0,0,0,TimeSpan.Zero) }
            );
        }
    }
}
