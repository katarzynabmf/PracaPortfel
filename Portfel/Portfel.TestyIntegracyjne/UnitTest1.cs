using Microsoft.EntityFrameworkCore;
using Portfel.Data;
using Portfel.Data.Data;
using Portfel.Data.Migrations;
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

            contextInMemory.Aktywa.Add(new Aktywo { Nazwa = "PZU", Symbol = "PZU", CenaAktualna = 100 });
            contextInMemory.SaveChanges();

            // ----------------------------------------------------------------------

            var portfelSerwis = new PortfelSerwis(contextInMemory);
            portfelSerwis.WplacSrodkiNaKonto(123, portfel.Entity);
            portfel.Entity.StanPortfelaAssert(123, 0);

            portfelSerwis.KupAktywo("PZU", 10, 10, portfel.Entity);
            portfel.Entity.StanPortfelaAssert(23, 1);

            portfelSerwis.KupAktywo("PZU", 1, 1, portfel.Entity);
            portfel.Entity.StanPortfelaAssert(22, 1);

            portfelSerwis.SprzedajAktywo("PZU", 11, 100, portfel.Entity);
            portfel.Entity.StanPortfelaAssert(1122, 0);

            portfelSerwis.WyplacSrodkiZKonta(500, portfel.Entity);
            portfel.Entity.StanPortfelaAssert(622, 0);

            //var uzytkownicy = contextInMemory.Uzytkownik.Include(u => u.Portfele).ToList();
            Assert.NotEmpty(portfel.Entity.Pozycje);
        }

        
    }

    public static class PortfelAsercje
    {
        public static void StanPortfelaAssert(this Data.Data.Portfel portfel, decimal iloscGotowki, int iloscAktyw)
        {
            Assert.Equal(iloscGotowki, portfel.KontoGotowkowe.StanKonta);
            Assert.Equal(iloscAktyw, portfel.Pozycje.Count);
        }
    }
}