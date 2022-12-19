using System.ComponentModel.DataAnnotations;

namespace Portfel.Data.Data
{
    public class ObiektBazodanowy
    {
        [Key] //to niżej jest kluczem głównym, samo się automatycznie inkrementuje
        public int Id { get; set; }

        [Display(Name = "Czy Aktywne")]
        public bool Aktywna { get; set; } = true;
    }

    public class Portfel : ObiektBazodanowy
    {
        public Portfel()
        {
        }

        public Portfel(string nazwa, Uzytkownik uzytkownik, string waluta = "USD")
        {
            Nazwa = nazwa;
            Uzytkownik = uzytkownik;
            Waluta = waluta;
            KontoGotowkowe = new KontoGotowkowe();
        }

        [Required]
        public string Nazwa { get; set; }
     
        public string Waluta { get; set; }
        [Display(Name = "Użytkownik")]
        public int? UzytkownikId { get; set; }
        public Uzytkownik Uzytkownik { get; set; }
        public KontoGotowkowe KontoGotowkowe { get; set; } 
        public ICollection<Pozycja> Pozycje { get; set; } = new List<Pozycja>();
        public ICollection<TransakcjaNew> Transakcje { get; set; } = new List<TransakcjaNew>();
    }


    public class Waluta : ObiektBazodanowy
    {
        public string Symbol { get; set; }
    }

    public class StworzPortfelRequest
    {
        public string Nazwa { get; set; }
    }



}
