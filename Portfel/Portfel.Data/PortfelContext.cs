﻿using Microsoft.EntityFrameworkCore;
using Portfel.Data.Data;

namespace Portfel.Data
{
    public class PortfelContext : DbContext
    {
        public PortfelContext()
        {
                
        }
        public PortfelContext(DbContextOptions options)
            : base(options)
        {
        }

        public virtual DbSet<Konto> Konto { get; set; }
        public virtual DbSet<RodzajOplaty> RodzajOplaty { get; set; }
        public virtual DbSet<RodzajTransakcji> RodzajTransakcji { get; set; }
        public virtual DbSet<SymbolGieldowy> SymbolGieldowy { get; set; }
        public virtual DbSet<Transakcja> Transakcja { get; set; }
        public virtual DbSet<Uzytkownik> Uzytkownik { get; set; }
        public virtual DbSet<Aktualnosc> Aktualnosc { get; set; }
        public virtual DbSet<Data.Portfel> Portfele { get; set; }
        public virtual DbSet<Data.Aktywo> Aktywa { get; set; }
        public virtual DbSet<Data.TransakcjaNew> TransakcjeNew { get; set; }
        public virtual DbSet<Pozycja> Pozycje { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Data.Portfel>().HasQueryFilter(portfel => portfel.Aktywna);
        }
    }
}