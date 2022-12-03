using Microsoft.EntityFrameworkCore;
using Portfel.Data;
using Portfel.Data.Data;
//using Portfel.Data.Migrations; todo
using Portfel.Data.Serwisy;

namespace Portfel.TestyIntegracyjne
{
    public class TestyPortfela
    {
        private PortfelContext _contextInMemory;
        
        [Fact]
        public void Test1()
        {
            var optionsBuilder = new DbContextOptionsBuilder<PortfelContext>();
            optionsBuilder.UseInMemoryDatabase("Portfel");
            _contextInMemory = new PortfelContext(optionsBuilder.Options);

            var uzytkownik = _contextInMemory.Uzytkownik.Add(new Uzytkownik(){DataUtworzenia = DateTime.Now, Email = "x", Haslo = "x", Imie = "y", Portfele = new List<Data.Data.Portfel>()});
            var portfel = _contextInMemory.Portfele.Add(new Data.Data.Portfel() { Nazwa = "Testowy", Waluta = "PLN" });
            uzytkownik.Entity.Portfele.Add(portfel.Entity);
            _contextInMemory.SaveChanges();

            _contextInMemory.Aktywa.Add(new Aktywo { Nazwa = "PZU", Symbol = "PZU", CenaAktualna = 100 });
            _contextInMemory.Aktywa.Add(new Aktywo { Nazwa = "Alior", Symbol = "ALR", CenaAktualna = 100 });
            _contextInMemory.SaveChanges();

            // ----------------------------------------------------------------------

            var portfelSerwis = new PortfelSerwis(_contextInMemory);
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

            // ----------------------------------------------------------------------------

            portfelSerwis.KupAktywo("ALR", 10, 10, portfel.Entity);
            portfel.Entity.StanPortfelaAssert(522, 1);

            Assert.Throws<InvalidOperationException>(() => portfelSerwis.SprzedajAktywo("ALR", 20, 100, portfel.Entity));
            portfel.Entity.StanPortfelaAssert(522, 1);

            // ----------------------------------------------------------------------------

            Assert.Throws<InvalidOperationException>(() => portfelSerwis.KupAktywo("PZU", 1, 700, portfel.Entity));
            portfel.Entity.StanPortfelaAssert(522, 1);
            //----------------------------------------------------------------------
            Assert.Throws<ArgumentException>(() => portfelSerwis.KupAktywo("PZU", 1, -700, portfel.Entity));
            portfel.Entity.StanPortfelaAssert(522, 1);
            //----------------------------------------------------------------------

            Assert.Throws<ArgumentException>(() => portfelSerwis.WplacSrodkiNaKonto(-100, portfel.Entity));
            portfel.Entity.StanPortfelaAssert(522, 1);
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