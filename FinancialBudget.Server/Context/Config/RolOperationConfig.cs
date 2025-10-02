using FinancialBudget.Server.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinancialBudget.Server.Context.Config
{
    public class RolOperationConfig : IEntityTypeConfiguration<RolOperation>
    {
        public void Configure(EntityTypeBuilder<RolOperation> entity)
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            entity.HasOne(e => e.Rol)
                .WithMany(e => e.RolOperations)
                .HasForeignKey(e => e.RolId);

            entity.HasOne(e => e.Operation)
                .WithMany(e => e.RolOperations)
                .HasForeignKey(e => e.OperationId);

            entity.HasData(
                new RolOperation
                {
                    Id = 1,
                    RolId = 1,
                    OperationId = 1,
                    State = 1,
                    CreatedAt = new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified),
                    CreatedBy = 1,
                    UpdatedAt = null,
                    UpdatedBy = null
                },
                new RolOperation
                {
                    Id = 2,
                    RolId = 1,
                    OperationId = 2,
                    State = 1,
                    CreatedAt = new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified),
                    CreatedBy = 1,
                    UpdatedAt = null,
                    UpdatedBy = null
                },
                new RolOperation
                {
                    Id = 3,
                    RolId = 1,
                    OperationId = 3,
                    State = 1,
                    CreatedAt = new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified),
                    CreatedBy = 1,
                    UpdatedAt = null,
                    UpdatedBy = null
                },
                new RolOperation
                {
                    Id = 4,
                    RolId = 1,
                    OperationId = 4,
                    State = 1,
                    CreatedAt = new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified),
                    CreatedBy = 1,
                    UpdatedAt = null,
                    UpdatedBy = null
                }
            );
        }
    }
}
