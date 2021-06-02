using Microsoft.EntityFrameworkCore;


namespace MinhaAPICoreDemo.Model
{
    public class ApiDbContext : DbContext
    {

        public ApiDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Fornecedor> Fornecedores { get; set; }

    }
}
