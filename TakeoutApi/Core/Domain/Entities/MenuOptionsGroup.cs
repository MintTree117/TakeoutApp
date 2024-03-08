using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.Domain.Entities;

public sealed class MenuOptionGroup
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public List<MenuOption> MenuOptions { get; set; } = [ ];
    public List<MenuItemOptionGroup> MenuItemOptionGroups { get; set; } = [ ];

    public class MenuOptionGroupConfiguration : IEntityTypeConfiguration<MenuOptionGroup>
    {
        public void Configure( EntityTypeBuilder<MenuOptionGroup> builder )
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