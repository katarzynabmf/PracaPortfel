﻿using Microsoft.EntityFrameworkCore;
using Portfel.Data.Data;

namespace Portfel.Data
{
    public class PortfelContext : DbContext
    {
        public PortfelContext(DbContextOptions options)
            : base(options)
        {
        }
        public DbSet<Konto> Konto { get; set; }
        public DbSet<RodzajOplaty> RodzajOplaty { get; set; }
        public DbSet<RodzajTransakcji> RodzajTransakcji { get; set; }
        public DbSet<SymbolGieldowy> SymbolGieldowy { get; set; }
        public DbSet<Transakcja> Transakcja { get; set; }
        public DbSet<Uzytkownik> Uzytkownik { get; set; }
    }
}