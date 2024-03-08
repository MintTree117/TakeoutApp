using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Domain.Entities;

public sealed class MenuItem
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public double Price { get; set; }
    public decimal? SalePrice { get; set; }
    public string? ImageUrl { get; set; } = string.Empty;

    public MenuCategory MenuCategory { get; set; } = default!;
    public int MenuCategoryId { get; set; }

    public List<MenuItemOptionGroup> MenuItemOptionGroups { get; set; } = [ ];

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
                .HasOne( i => i.MenuCategory )
                .WithMany( c => c.MenuItems )
                .HasForeignKey( i => i.MenuCategoryId )
                .IsRequired();
        }
    }
}