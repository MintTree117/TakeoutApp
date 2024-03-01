using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Domain.Entities;

public sealed class MenuCategory
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public class MenuCategoryConfiguration : IEntityTypeConfiguration<MenuCategory>
    {
        public void Configure( EntityTypeBuilder<MenuCategory> builder )
        {
            builder
                .Property( i => i.Id )
                .IsRequired();
            builder
                .Property( i => i.Name )
                .IsRequired()
                .HasMaxLength( 64 );
        }
    }
}