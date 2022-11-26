using Microsoft.EntityFrameworkCore;
using Portfel.Data.Data;

namespace Portfel.Data
{
    public class PortfelContexts : DbContext
    {
        public PortfelContexts(DbContextOptions options)
            : base(options)
        {
        }
        public DbSet<Konto> Konto { get; set; }
        public DbSet<RodzajOplaty> RodzajOplaty { get; set; }
        public DbSet<RodzajTransakcji> RodzajTransakcji { get; set; }
        public DbSet<SymbolGieldowy> SymbolGieldowy { get; set; }
        public DbSet<Transakcja> Transakcja { get; set; }
        public DbSet<Uzytkownik> Uzytkownik { get; set; }
        public DbSet<Aktualnosc> Aktualnosc { get; set; }
        public DbSet<Data.Portfel> Portfele { get; set; }
        public DbSet<Data.Aktywo> Aktywa { get; set; }
        public DbSet<Data.TransakcjaNew> TransakcjeNew { get; set; }
        public DbSet<Pozycja> Pozycje { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Data.Portfel>().HasQueryFilter(portfel => portfel.Aktywna);
        }
    }
}