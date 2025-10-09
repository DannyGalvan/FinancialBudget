using FinancialBudget.Server.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinancialBudget.Server.Context.Config
{
    public class OperationConfig : IEntityTypeConfiguration<Operation>
    {
        public void Configure(EntityTypeBuilder<Operation> entity)
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Name)
                .HasMaxLength(255);
            entity.Property(e => e.Policy)
                .HasMaxLength(255);
            entity.Property(e => e.Icon)
                .HasMaxLength(255);
            entity.Property(e => e.Path)
                .HasMaxLength(255);
            entity.HasOne(e => e.Module)
                .WithMany(e => e.Operations)
                .HasForeignKey(e => e.ModuleId);

            entity.HasOne(e => e.Module)
                .WithMany(e => e.Operations)
                .HasForeignKey(e => e.ModuleId);

            entity.HasData(
                new Operation
                {
                    Id = 1,
                    Name = "Solicitudes Mantenimiento",
                    Policy = "Request.Maintenance",
                    Icon = "bi bi-wrench",
                    Path = "/Request/Maintenance",
                    ModuleId = 1,
                    IsVisible = true,
                    State = 1,
                    CreatedAt = new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified),
                    CreatedBy = 1,
                    UpdatedAt = null,
                    UpdatedBy = null
                },
                new Operation
                {
                    Id = 2,
                    Name = "Solicitudes Eventos",
                    Policy = "Request.Events",
                    Icon = "bi bi-calendar-event",
                    Path = "/Request/Events",
                    ModuleId = 1,
                    IsVisible = true,
                    State = 1,
                    CreatedAt = new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified),
                    CreatedBy = 1,
                    UpdatedAt = null,
                    UpdatedBy = null
                },
                new Operation
                {
                    Id = 3,
                    Name = "Reporte Financiero",
                    Policy = "Report.Financial",
                    Icon = "bi bi-graph-up",
                    Path = "/Report/Financial",
                    ModuleId = 1,
                    IsVisible = true,
                    State = 1,
                    CreatedAt = new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified),
                    CreatedBy = 1,
                    UpdatedAt = null,
                    UpdatedBy = null
                },
                new Operation
                {
                    Id = 4,
                    Name = "Reporte Resumen Ciudadania",
                    Policy = "Report.SummaryCitizenship",
                    Icon = "bi bi-people",
                    Path = "/Report/SummaryCitizenship",
                    ModuleId = 1,
                    IsVisible = true,
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
