using FinancialBudget.Server.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinancialBudget.Server.Context.Config
{
    public class SplitTypeConfig : IEntityTypeConfiguration<SplitType>
    {
        public void Configure(EntityTypeBuilder<SplitType> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasData(
                new SplitType { Id = 1, Name = "Debito", State = 1, CreatedBy = 1, CreatedAt = new DateTimeOffset(2025,10,1,0,0,0,TimeSpan.Zero) },
                new SplitType { Id = 2, Name = "Credito", State = 1, CreatedBy = 1, CreatedAt = new DateTimeOffset(2025,10,1,0,0,0,TimeSpan.Zero) }
            );

        }
    }
}
