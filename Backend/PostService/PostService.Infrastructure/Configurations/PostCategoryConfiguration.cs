using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PostService.Domain.Models;

namespace PostService.Infrastructure.Configurations;

public class PostCategoryConfiguration : IEntityTypeConfiguration<PostCategory>
{
    public void Configure(EntityTypeBuilder<PostCategory> builder)
    {
        builder.ToTable("PostCategoryMap");
        builder.HasKey(x => new { x.CategoryId, x.PostId });

        builder.HasOne(x => x.Post)
            .WithMany(x => x.PostCategories)
            .HasForeignKey(x => x.PostId);

        builder.HasOne(x => x.Category)
            .WithMany(x => x.PostCategories)
            .HasForeignKey(x => x.CategoryId);
    }
}