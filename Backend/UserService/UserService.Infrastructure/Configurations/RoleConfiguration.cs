using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain;
using UserService.Domain.Models;

namespace UserService.Infrastructure.Configurations;

/// <summary>
/// Конфигуратор для роли.
/// </summary>
public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    /// <summary>
    /// Конфигурация.
    /// </summary>
    /// <param name="builder"><see cref="EntityTypeBuilder"/>.</param>
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(x => x.RoleId);
        builder.Property(x => x.RoleId).IsRequired();
        builder.Property(x => x.RoleName).IsRequired();

        builder.HasData(
            new Role { RoleId = Roles.User, RoleName = "User" },
            new Role { RoleId = Roles.Admin, RoleName = "Admin" }
        );
    }
}