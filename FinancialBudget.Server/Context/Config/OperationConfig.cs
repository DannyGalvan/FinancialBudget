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
                    Policy = "Request.Create",
                    Icon = "bi bi-cash",
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
                    Policy = "Request.Create",
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
                    Name = "Actualizacion de solicitudes",
                    Policy = "Request.Update",
                    Icon = "",
                    Path = "/Request/Update",
                    ModuleId = 1,
                    IsVisible = false,
                    State = 1,
                    CreatedAt = new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified),
                    CreatedBy = 1,
                    UpdatedAt = null,
                    UpdatedBy = null
                },
                new Operation
                {
                    Id = 4,
                    Name = "Eliminacion de solicitudes",
                    Policy = "Request.Delete",
                    Icon = "",
                    Path = "/Request/Events",
                    ModuleId = 1,
                    IsVisible = false,
                    State = 1,
                    CreatedAt = new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified),
                    CreatedBy = 1,
                    UpdatedAt = null,
                    UpdatedBy = null
                },
                new Operation
                {
                    Id = 5,
                    Name = "Consulta de solicitudes",
                    Policy = "Request.List",
                    Icon = "",
                    Path = "/Request/List",
                    ModuleId = 1,
                    IsVisible = false,
                    State = 1,
                    CreatedAt = new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified),
                    CreatedBy = 1,
                    UpdatedAt = null,
                    UpdatedBy = null
                },
                new Operation
                {
                    Id = 6,
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
                    Id = 7,
                    Name = "Reporte Resumen Ciudadania",
                    Policy = "Report.SummaryCitizenship",
                    Icon = "bi bi-list",
                    Path = "/Report/SummaryCitizenship",
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
                    Id = 8,
                    Name = "Crear Catalogo",
                    Policy = "Catalogue.Create",
                    Icon = "",
                    Path = "/Catalogue/Create",
                    ModuleId = 1,
                    IsVisible = false,
                    State = 1,
                    CreatedAt = new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified),
                    CreatedBy = 1,
                    UpdatedAt = null,
                    UpdatedBy = null
                },
                new Operation
                {
                    Id = 9,
                    Name = "Actualizar Catalogo",
                    Policy = "Catalogue.Update",
                    Icon = "",
                    Path = "/Catalogue/Update",
                    ModuleId = 1,
                    IsVisible = false,
                    State = 1,
                    CreatedAt = new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified),
                    CreatedBy = 1,
                    UpdatedAt = null,
                    UpdatedBy = null
                },
                new Operation
                {
                    Id = 10,
                    Name = "Eliminar Catalogo",
                    Policy = "Catalogue.Delete",
                    Icon = "",
                    Path = "/Catalogue/Delete",
                    ModuleId = 1,
                    IsVisible = false,
                    State = 1,
                    CreatedAt = new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified),
                    CreatedBy = 1,
                    UpdatedAt = null,
                    UpdatedBy = null
                },
                new Operation
                {
                    Id = 11,
                    Name = "Crear Presupuesto",
                    Policy = "Budget.Create",
                    Icon = "bi bi-cash",
                    Path = "/Budget/Create",
                    ModuleId = 1,
                    IsVisible = false,
                    State = 1,
                    CreatedAt = new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified),
                    CreatedBy = 1,
                    UpdatedAt = null,
                    UpdatedBy = null
                },
                new Operation
                {
                    Id = 12,
                    Name = "Actualizar Presupuesto",
                    Policy = "Budget.Update",
                    Icon = "bi bi-calendar-event",
                    Path = "/Budget/Update",
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
                    Id = 13,
                    Name = "Eliminar Presupuesto",
                    Policy = "Budget.Delete",
                    Icon = "",
                    Path = "/Budget/Delete",
                    ModuleId = 1,
                    IsVisible = false,
                    State = 1,
                    CreatedAt = new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified),
                    CreatedBy = 1,
                    UpdatedAt = null,
                    UpdatedBy = null
                },
                new Operation
                {
                    Id = 14,
                    Name = "Listar Presupuesto",
                    Policy = "Budget.List",
                    Icon = "",
                    Path = "/Budget/List",
                    ModuleId = 1,
                    IsVisible = false,
                    State = 1,
                    CreatedAt = new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified),
                    CreatedBy = 1,
                    UpdatedAt = null,
                    UpdatedBy = null
                },
                new Operation
                {
                    Id = 15,
                    Name = "Crear Partida Presupuestaria",
                    Policy = "BudgetItem.Create",
                    Icon = "bi bi-cash",
                    Path = "/BudgetItem/Create",
                    ModuleId = 1,
                    IsVisible = false,
                    State = 1,
                    CreatedAt = new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified),
                    CreatedBy = 1,
                    UpdatedAt = null,
                    UpdatedBy = null
                },
                new Operation
                {
                    Id = 16,
                    Name = "Actualizar Partida Presupuestaria",
                    Policy = "BudgetItem.Update",
                    Icon = "bi bi-calendar-event",
                    Path = "/BudgetItem/Update",
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
                    Id = 17,
                    Name = "Eliminar Partida Presupuestaria",
                    Policy = "BudgetItem.Delete",
                    Icon = "",
                    Path = "/BudgetItem/Delete",
                    ModuleId = 1,
                    IsVisible = false,
                    State = 1,
                    CreatedAt = new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified),
                    CreatedBy = 1,
                    UpdatedAt = null,
                    UpdatedBy = null
                },
                new Operation
                {
                    Id = 18,
                    Name = "Listar Partida Presupuestaria",
                    Policy = "BudgetItem.List",
                    Icon = "",
                    Path = "/BudgetItem/List",
                    ModuleId = 1,
                    IsVisible = false,
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
