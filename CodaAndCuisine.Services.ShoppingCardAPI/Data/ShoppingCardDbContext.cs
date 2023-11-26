using CodaAndCuisine.Services.ShoppingCartAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace CodeAndCuisine.Services.ShoppingCartAPI.Data
{
    public class ShoppingCartDbContext : DbContext
    {
        public ShoppingCartDbContext(DbContextOptions<ShoppingCartDbContext> options) : base(options)
        {
        }

        public DbSet<CartHeader> CartHeaders{ get; set; }
        public DbSet<CartDetail> CartDetails{ get; set; }

    }
}
