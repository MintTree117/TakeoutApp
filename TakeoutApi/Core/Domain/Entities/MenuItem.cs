using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Domain.Entities;

public sealed class MenuItem
{
    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public decimal? SalePrice { get; private set; }
    public string? ImageUrl { get; private set; } = string.Empty;

    public MenuCategory MenuCategory { get; private set; } = null!;
    public int MenuCategoryId { get; private set; }

    public List<MenuOptionGroup> MenuOptionGroups { get; set; } = [ ];

    public class MenuItemConfiguration : IEntityTypeConfiguration<MenuItem>
    {
        public void Configure( EntityTypeBuilder<MenuItem> builder )
        {
            builder
                .Property( i => i.Id )
                .IsRequired();
            builder
                .Property( i => i.Name )
                .IsRequired()
                .HasMaxLength( 64 );
            builder
                .Property( i => i.ImageUrl )
                .HasMaxLength( 120 );
            builder
                .Property( i => i.Price )
                .IsRequired()
                .HasColumnType( "decimal(18,2)" );
            builder
                .Property( i => i.SalePrice )
                .HasColumnType( "decimal(18,2)" );
            builder
                .HasOne( c => c.MenuCategory )
                .WithMany()
                .HasForeignKey( i => i.MenuCategoryId );
            builder
                .HasMany( i => i.MenuOptionGroups )
                .WithMany( g => g.MenuItems );
        }
    }
}