using Microsoft.EntityFrameworkCore;


namespace MinhaAPICoreDemo.Model
{
    public class ProductsContext : DbContext
    {

        public ProductsContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }

    }
}
