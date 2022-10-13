using Microsoft.EntityFrameworkCore;
using Portfel.Data.Data;

namespace Portfel.Data
{
    public class PortfelContext : DbContext
    {
        public PortfelContext(DbContextOptions options)
            : base(options)
        {
        }
        public DbSet<Konto> Kontos { get; set; }
    }
}