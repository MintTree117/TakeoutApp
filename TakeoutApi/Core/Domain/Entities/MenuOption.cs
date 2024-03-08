using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Domain.Entities;

public sealed class MenuOption
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal? SalePrice { get; set; }

    public MenuOptionGroup MenuOptionGroup { get; set; } = null!;
    public int MenuOptionGroupId { get; set; }

    public class MenuOptionConfiguration : IEntityTypeConfiguration<MenuOption>
    {
        public void Configure( EntityTypeBuilder<MenuOption> builder )
        {
            builder
                .Property( o => o.Id )
                .IsRequired();
            builder
                .Property( o => o.Name )
                .IsRequired()
                .HasMaxLength( 64 );
            builder
                .Property( o => o.Price )
                .IsRequired()
                .HasColumnType( "decimal(18,2)" );
            builder
                .Property( o => o.SalePrice )
                .HasColumnType( "decimal(18,2)" );
            builder
                .HasOne( o => o.MenuOptionGroup )
                .WithMany( g => g.MenuOptions )
                .HasForeignKey( o => o.MenuOptionGroupId );
        }
    }
}