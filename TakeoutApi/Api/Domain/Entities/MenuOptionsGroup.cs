using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Domain.Entities;

public sealed class MenuOptionGroup
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public List<MenuItem> MenuItems { get; set; } = [ ];

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