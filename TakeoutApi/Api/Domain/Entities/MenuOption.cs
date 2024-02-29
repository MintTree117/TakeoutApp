using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Domain.Entities;

public sealed class MenuOption
{
    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public decimal? SalePrice { get; private set; }

    public MenuOptionGroup MenuOptionGroup { get; private set; } = null!;
    public int MenuOptionGroupId { get; private set; }

    public class MenuOptionConfiguration : IEntityTypeConfiguration<MenuOption>
    {
        public void Configure( EntityTypeBuilder<MenuOption> builder )
        {
            builder
                .Property( i => i.Id )
                .IsRequired();
            builder
                .Property( i => i.Name )
                .IsRequired()
                .HasMaxLength( 64 );
            builder
                .Property( i => i.Price )
                .IsRequired()
                .HasColumnType( "decimal(18,2)" );
            builder
                .Property( i => i.SalePrice )
                .HasColumnType( "decimal(18,2)" );
            builder
                .HasOne( c => c.MenuOptionGroup )
                .WithMany()
                .HasForeignKey( i => i.MenuOptionGroupId );
        }
    }
}