﻿using FinancialBudget.Server.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinancialBudget.Server.Context.Config
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> entity)
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Name)
                .HasMaxLength(255);
            entity.Property(e => e.Password)
                .HasMaxLength(255);
            entity.Property(e => e.Email)
                .HasMaxLength(255);

            entity.HasOne(e => e.Rol)
                .WithMany(e => e.Users)
                .HasForeignKey(e => e.RolId);

            //password: Guatemala1.
            entity.HasData(
                new User
                {
                    Id = 1,
                    RolId = 1,
                    Password = "$2a$12$86Ty8oUVWKPbU8JqCII9VO.FgM1C10dweQ4xKhM4jj1LWL9jwNu7.",
                    Name = "Super Administrador",
                    Email = "pruebas.test29111999@gmail.com",
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
