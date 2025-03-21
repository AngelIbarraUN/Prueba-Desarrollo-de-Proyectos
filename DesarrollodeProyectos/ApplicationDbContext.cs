
using DesarrollodeProyectos.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Cap> Caps { get; set; } 
          public DbSet<Shirt> Shirts { get; set; } 
          public DbSet<Sweater> Sweaters { get; set; } 
          public DbSet<Material> Materials { get; set; }
          public DbSet<Category> Categories { get; set; }
          
          public DbSet<Size> Sizes { get; set; }
    
}