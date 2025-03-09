using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PostService.Domain.Models;

namespace PostService.Infrastructure.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Category");
        builder.HasIndex(x => x.Id).IsUnique();
        builder.Property(x => x.Id).IsRequired();
        builder.Property(x => x.Name).IsRequired();
    }
}