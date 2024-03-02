using Api.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Persistence;

public sealed class EfContext : IdentityDbContext
{
    public EfContext( DbContextOptions<EfContext> options ) : base( options )
    {

    }

    public DbSet<MenuCategory> MenuCategories { get; set; }
    public DbSet<MenuItem> MenuItems { get; set; }
    public DbSet<MenuOption> MenuOptions { get; set; }
    public DbSet<MenuOptionGroup> MenuOptionGroups { get; set; }

    protected override void OnModelCreating( ModelBuilder modelBuilder )
    {
        base.OnModelCreating( modelBuilder );
        modelBuilder.ApplyConfiguration( new MenuCategory.MenuCategoryConfiguration() );
        modelBuilder.ApplyConfiguration( new MenuItem.MenuItemConfiguration() );
        modelBuilder.ApplyConfiguration( new MenuOption.MenuOptionConfiguration() );
        modelBuilder.ApplyConfiguration( new MenuOptionGroup.MenuOptionGroupConfiguration() );
    }
}