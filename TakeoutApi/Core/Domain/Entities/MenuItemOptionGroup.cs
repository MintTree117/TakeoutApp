using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Domain.Entities;

public sealed class MenuItemOptionGroup
{
    public int MenuItemId { get; set; }
    public MenuItem MenuItem { get; set; } = default!;
    
    public int MenuOptionGroupId { get; set; }
    public MenuOptionGroup MenuOptionGroup { get; set; } = default!;

    public class MenuItemOptionGroupConfiguration : IEntityTypeConfiguration<MenuItemOptionGroup>
    {
        public void Configure( EntityTypeBuilder<MenuItemOptionGroup> builder )
        {
            builder.HasKey( mog => new { mog.MenuItemId, mog.MenuOptionGroupId } );

            builder.HasOne( mog => mog.MenuItem )
                   .WithMany( mi => mi.MenuItemOptionGroups )
                   .HasForeignKey( mog => mog.MenuItemId );

            builder.HasOne( mog => mog.MenuOptionGroup )
                   .WithMany( mog => mog.MenuItemOptionGroups )
                   .HasForeignKey( mog => mog.MenuOptionGroupId );
        }
    }
}