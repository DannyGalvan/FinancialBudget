using FinancialBudget.Server.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinancialBudget.Server.Context.Config
{
    public class RequestStatusConfig : IEntityTypeConfiguration<RequestStatus>
    {
        public void Configure(EntityTypeBuilder<RequestStatus> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasData(
                new RequestStatus { Id = 1, Name = "Pendiente", State = 1, CreatedBy = 1, CreatedAt = new DateTimeOffset(2025,10,1,0,0,0,TimeSpan.Zero) },
                new RequestStatus { Id = 2, Name = "Aprobado", State = 1, CreatedBy = 1, CreatedAt = new DateTimeOffset(2025,10,1,0,0,0,TimeSpan.Zero) },
                new RequestStatus { Id = 3, Name = "Rechazado", State = 1, CreatedBy = 1, CreatedAt = new DateTimeOffset(2025,10,1,0,0,0,TimeSpan.Zero) }
            );
        }
    }
}
