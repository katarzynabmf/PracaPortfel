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
        [Required]
        public string Nazwa { get; set; }
        [Required]
        public string Waluta { get; set; }
        [Display(Name = "Użytkownik")]
        public int? UzytkownikId { get; set; }
        public Uzytkownik Uzytkownik { get; set; }
        public int KontoGotowkoweId { get; set; }
        public KontoGotowkowe KontoGotowkowe { get; set; } = new KontoGotowkowe();
        public ICollection<Pozycja> Pozycje { get; set; } = new List<Pozycja>();
        public ICollection<TransakcjaNew> Transakcje { get; set; } = new List<TransakcjaNew>();
    }

    public class TransakcjaNew : ObiektBazodanowy
    {
        public TransakcjaNew()
        {
            
        }
        public TransakcjaNew(Aktywo aktywo, Kierunek kierunek, decimal cena, uint ilosc)
        {
            Aktywo = aktywo;
            Kierunek = kierunek;
            Cena = cena;
            Ilosc = ilosc;
            DataTransakcji = DateTime.Now;
        }
        public Kierunek Kierunek { get; set; }
        public Aktywo Aktywo { get; set; }
        public Portfel Portfel { get; set; }
        public decimal Cena { get; set; }
        public uint Ilosc { get; }
        public DateTime DataTransakcji { get; set; }
        public string Komentarz { get; set; } = "";
    }

    public enum Kierunek
    {
        Kupno, Sprzedaz
    }

    public class Pozycja : ObiektBazodanowy
    {
        public Aktywo Aktywo { get; set; }
        public uint Ilosc { get; set; }
        public decimal SredniaCenaZakupu { get; set; }
    }


    public class Aktywo : ObiektBazodanowy
    {
        public string Symbol { get; set; }
        public string Nazwa { get; set; }
        public decimal CenaAktualna { get; set; }
    }

    public class KontoGotowkowe : ObiektBazodanowy
    {
        [Key] //to niżej jest kluczem głównym, samo się automatycznie inkrementuje
        public int Id { get; set; }
        public Portfel Portfel { get; set; }
        public decimal StanKonta { get; set; }
        public ICollection<OperacjaGotowkowa> OperacjeGotowkowe { get; set; } = new List<OperacjaGotowkowa>();
    }

    public class OperacjaGotowkowa : ObiektBazodanowy
    {
        public OperacjaGotowkowa(TypOperacjiGotowkowej typOperacjiGotowkowej, decimal kwota)
        {
            Kwota = kwota;
            TypOperacjiGotowkowej = typOperacjiGotowkowej;
            DataOperacji = DateTime.Now;
        }

        public void Wykonaj()
        {
            switch (TypOperacjiGotowkowej)
            {
                case TypOperacjiGotowkowej.Wplata or TypOperacjiGotowkowej.Uznanie:
                    KontoGotowkowe.StanKonta += Kwota;
                    break;
                case TypOperacjiGotowkowej.Wyplata or TypOperacjiGotowkowej.Obciazenie:
                    if (KontoGotowkowe.StanKonta - Kwota < 0)
                        throw new InvalidOperationException("Brak wystarczających środków.");
                    KontoGotowkowe.StanKonta -= Kwota;
                    break;
            }
        }

        public KontoGotowkowe KontoGotowkowe { get; set; }
        public DateTime DataOperacji { get; set; }
        public decimal Kwota { get; set; }
        public TypOperacjiGotowkowej TypOperacjiGotowkowej { get; set; }
    }

    public enum TypOperacjiGotowkowej
    {
        Wplata, Wyplata, Obciazenie, Uznanie
    }

    public class Waluta : ObiektBazodanowy
    {
        public string Symbol { get; set; }
    }
}
