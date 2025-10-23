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
                //Rol Administrador tiene todas las operaciones
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
                },
                new RolOperation
                {
                    Id = 5,
                    RolId = 1,
                    OperationId = 5,
                    State = 1,
                    CreatedAt = new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified),
                    CreatedBy = 1,
                    UpdatedAt = null,
                    UpdatedBy = null
                },
                new RolOperation
                {
                    Id = 6,
                    RolId = 1,
                    OperationId = 6,
                    State = 1,
                    CreatedAt = new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified),
                    CreatedBy = 1,
                    UpdatedAt = null,
                    UpdatedBy = null
                },
                new RolOperation
                {
                    Id = 7,
                    RolId = 1,
                    OperationId = 7,
                    State = 1,
                    CreatedAt = new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified),
                    CreatedBy = 1,
                    UpdatedAt = null,
                    UpdatedBy = null
                },
                new RolOperation
                {
                    Id = 8,
                    RolId = 1,
                    OperationId = 8,
                    State = 1,
                    CreatedAt = new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified),
                    CreatedBy = 1,
                    UpdatedAt = null,
                    UpdatedBy = null
                },
                new RolOperation
                {
                    Id = 9,
                    RolId = 1,
                    OperationId = 9,
                    State = 1,
                    CreatedAt = new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified),
                    CreatedBy = 1,
                    UpdatedAt = null,
                    UpdatedBy = null
                },
                new RolOperation
                {
                    Id = 10,
                    RolId = 1,
                    OperationId = 10,
                    State = 1,
                    CreatedAt = new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified),
                    CreatedBy = 1,
                    UpdatedAt = null,
                    UpdatedBy = null
                },
                new RolOperation
                {
                    Id = 11,
                    RolId = 1,
                    OperationId = 11,
                    State = 1,
                    CreatedAt = new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified),
                    CreatedBy = 1,
                    UpdatedAt = null,
                    UpdatedBy = null
                },
                new RolOperation
                {
                    Id = 12,
                    RolId = 1,
                    OperationId = 12,
                    State = 1,
                    CreatedAt = new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified),
                    CreatedBy = 1,
                    UpdatedAt = null,
                    UpdatedBy = null
                },
                new RolOperation
                {
                    Id = 13,
                    RolId = 1,
                    OperationId = 13,
                    State = 1,
                    CreatedAt = new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified),
                    CreatedBy = 1,
                    UpdatedAt = null,
                    UpdatedBy = null
                },
                new RolOperation
                {
                    Id = 14,
                    RolId = 1,
                    OperationId = 14,
                    State = 1,
                    CreatedAt = new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified),
                    CreatedBy = 1,
                    UpdatedAt = null,
                    UpdatedBy = null
                },
                new RolOperation
                {
                    Id = 15,
                    RolId = 1,
                    OperationId = 15,
                    State = 1,
                    CreatedAt = new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified),
                    CreatedBy = 1,
                    UpdatedAt = null,
                    UpdatedBy = null
                },
                new RolOperation
                {
                    Id = 16,
                    RolId = 1,
                    OperationId = 16,
                    State = 1,
                    CreatedAt = new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified),
                    CreatedBy = 1,
                    UpdatedAt = null,
                    UpdatedBy = null
                },
                new RolOperation
                {
                    Id = 17,
                    RolId = 1,
                    OperationId = 17,
                    State = 1,
                    CreatedAt = new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified),
                    CreatedBy = 1,
                    UpdatedAt = null,
                    UpdatedBy = null
                },
                new RolOperation
                {
                    Id = 18,
                    RolId = 1,
                    OperationId = 18,
                    State = 1,
                    CreatedAt = new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified),
                    CreatedBy = 1,
                    UpdatedAt = null,
                    UpdatedBy = null
                },
                //Rol Participacion Ciudadana
                new RolOperation
                {
                    Id = 19,
                    RolId = 4,
                    OperationId = 14,
                    State = 1,
                    CreatedAt = new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified),
                    CreatedBy = 1,
                    UpdatedAt = null,
                    UpdatedBy = null
                },
                new RolOperation
                {
                    Id = 20,
                    RolId = 4,
                    OperationId = 18,
                    State = 1,
                    CreatedAt = new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified),
                    CreatedBy = 1,
                    UpdatedAt = null,
                    UpdatedBy = null
                },
                //Rol Eventos
                new RolOperation
                {
                    Id = 21,
                    RolId = 2,
                    OperationId = 1,
                    State = 1,
                    CreatedAt = new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified),
                    CreatedBy = 1,
                    UpdatedAt = null,
                    UpdatedBy = null
                },
                new RolOperation
                {
                    Id = 22,
                    RolId = 2,
                    OperationId = 2,
                    State = 1,
                    CreatedAt = new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified),
                    CreatedBy = 1,
                    UpdatedAt = null,
                    UpdatedBy = null
                },
                new RolOperation
                {
                    Id = 23,
                    RolId = 2,
                    OperationId = 5,
                    State = 1,
                    CreatedAt = new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified),
                    CreatedBy = 1,
                    UpdatedAt = null,
                    UpdatedBy = null
                },
                //Rol Mantenimiento
                 new RolOperation
                 {
                     Id = 24,
                     RolId = 3,
                     OperationId = 1,
                     State = 1,
                     CreatedAt = new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified),
                     CreatedBy = 1,
                     UpdatedAt = null,
                     UpdatedBy = null
                 },
                 new RolOperation
                 {
                     Id = 25,
                     RolId = 3,
                     OperationId = 2,
                     State = 1,
                     CreatedAt = new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified),
                     CreatedBy = 1,
                     UpdatedAt = null,
                     UpdatedBy = null
                 },
                new RolOperation
                {
                    Id = 26,
                    RolId = 3,
                    OperationId = 5,
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
