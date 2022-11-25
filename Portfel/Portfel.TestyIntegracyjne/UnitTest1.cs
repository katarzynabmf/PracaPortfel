using Microsoft.EntityFrameworkCore;
using Portfel.Data;
using Portfel.Data.Data;
using Portfel.Data.Serwisy;

namespace Portfel.TestyIntegracyjne
{
    public class UnitTest1
    {
        private PortfelContexts contextInMemory;
        
        [Fact]
        public void Test1()
        {
            var optionsBuilder = new DbContextOptionsBuilder<PortfelContexts>();
            optionsBuilder.UseInMemoryDatabase("Portfel");
            contextInMemory = new PortfelContexts(optionsBuilder.Options);

            var uzytkownik = contextInMemory.Uzytkownik.Add(new Uzytkownik(){DataUtworzenia = DateTime.Now, Email = "x", Haslo = "x", Imie = "y"});
            var portfel = contextInMemory.Portfele.Add(new Data.Data.Portfel() { Nazwa = "Testowy", Waluta = "PLN" });
            uzytkownik.Entity.Portfele.Add(portfel.Entity);
            contextInMemory.SaveChanges();

            var portfelSerwis = new PortfelSerwis(contextInMemory);
            portfelSerwis.WplacSrodkiNaKonto(123, portfel.Entity);

            contextInMemory.Aktywa.Add(new Aktywo(){Nazwa = "PZU", Symbol = "PZU", CenaAktualna = 100});
            contextInMemory.SaveChanges();

            portfelSerwis.KupAktywo("PZU", 10, 10, portfel.Entity);
            portfelSerwis.KupAktywo("PZU", 1, 1, portfel.Entity);

            var uzytkownicy = contextInMemory.Uzytkownik.Include(u => u.Portfele).ToList();
            Assert.NotEmpty(uzytkownicy);
        }
    }
}