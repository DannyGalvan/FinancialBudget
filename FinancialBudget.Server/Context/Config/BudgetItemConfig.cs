using FinancialBudget.Server.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinancialBudget.Server.Context.Config
{
    public class BudgetItemConfig : IEntityTypeConfiguration<BudgetItem>
    {
        public void Configure(EntityTypeBuilder<BudgetItem> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.Amount)
                .HasPrecision(18, 2);

            builder.HasOne(x => x.Budget)
                .WithMany(b => b.BudgetItems)
                .HasForeignKey(x => x.BudgetId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_BudgetItem_Budget");

            builder.HasOne(x => x.Origin)
                .WithMany(x => x.BudgetItems)
                .HasForeignKey(x => x.OriginId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_BudgetItem_Origin");

            builder.HasOne(x => x.SplitType)
                .WithMany(x => x.BudgetItems)
                .HasForeignKey(x => x.SplitTypeId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_BudgetItem_SplitType");

            builder.HasOne(x => x.Request)
                .WithMany(x => x.BudgetItems)
                .HasForeignKey(x => x.RequestId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_BudgetItem_Request");
        }
    }
}
