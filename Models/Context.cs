using Microsoft.EntityFrameworkCore;

namespace ApiMarket.Models
{
    public class Context : DbContext
    {
        

            public Context(DbContextOptions<Context> options) : base(options)
            {
            }
            public DbSet<Productos> Productos { get; set; }
            public DbSet<Categorias> Categorias { get; set; }
            public DbSet<Usuarios> Usuarios { get; set; }
            public DbSet<ProductosCategorias> ProductosCategorias { get; set; }

          
    }
}

