using Microsoft.EntityFrameworkCore;
using Portfel.Data;
using Portfel.Data.Data;
using Portfel.Data.Serwisy;

namespace Portfel.TestyIntegracyjne;

public class TestyAktywow
{
    private PortfelContext _contextInMemory;

    [Fact]
    public void Test()
    {
        var optionsBuilder = new DbContextOptionsBuilder<PortfelContext>();
        optionsBuilder.UseInMemoryDatabase("Portfel");
        _contextInMemory = new PortfelContext(optionsBuilder.Options);

        var uzytkownik = _contextInMemory.Uzytkownik.Add(new Uzytkownik() { DataUtworzenia = DateTime.Now, Email = "x", Haslo = "x", Imie = "y", Portfele = new List<Data.Data.Portfel>() });
        var portfel = _contextInMemory.Portfele.Add(new Data.Data.Portfel() { Nazwa = "Testowy", Waluta = "PLN" });
        uzytkownik.Entity.Portfele.Add(portfel.Entity);
        _contextInMemory.SaveChanges();

        _contextInMemory.Aktywa.Add(new Aktywo { Nazwa = "Apple", Symbol = "AAPL", CenaAktualna = 100 });
        _contextInMemory.SaveChanges();
    }

    [Fact]
    public async Task Ajfdjodsj() //todo
    {
        // ----------------------------------------------------------------------
        //var aktywaSerwis = new SymboleSerwis(_contextInMemory);
        //await aktywaSerwis.ZaktualizujCeny();
    }
}