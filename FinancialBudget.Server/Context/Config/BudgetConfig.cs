using FinancialBudget.Server.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinancialBudget.Server.Context.Config
{
    public class BudgetConfig : IEntityTypeConfiguration<Budget>
    {
        public void Configure(EntityTypeBuilder<Budget> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.AuthorizedAmount)
                .HasPrecision(18, 2);

            builder.Property(x => x.AvailableAmount)
                .HasPrecision(18, 2);

            builder.Property(x => x.CommittedAmount)
                .HasPrecision(18, 2);
        }
    }
}
