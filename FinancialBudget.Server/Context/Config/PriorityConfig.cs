using FinancialBudget.Server.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinancialBudget.Server.Context.Config
{
    public class PriorityConfig : IEntityTypeConfiguration<Priority>
    {
        public void Configure(EntityTypeBuilder<Priority> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasData(
                new Priority { Id = 1, Name = "Baja", State = 1, CreatedBy = 1, CreatedAt = new DateTimeOffset(2025,10,1,0,0,0,TimeSpan.Zero) },
                new Priority { Id = 2, Name = "Media", State = 1, CreatedBy = 1, CreatedAt = new DateTimeOffset(2025,10,1,0,0,0,TimeSpan.Zero) },
                new Priority { Id = 3, Name = "Alta", State = 1, CreatedBy = 1, CreatedAt = new DateTimeOffset(2025,10,1,0,0,0,TimeSpan.Zero) }
            );
        }
    }
}
