using System.ComponentModel.DataAnnotations;

namespace Portfel.Data.Data;

public class TransakcjaNew : ObiektBazodanowy
{
    public TransakcjaNew()
    {
            
    }
    public TransakcjaNew(Aktywo aktywo, Kierunek kierunek, decimal cena, uint ilosc, string komentarz)
    {
        Aktywo = aktywo;
        Kierunek = kierunek;
        Cena = cena;
        Ilosc = ilosc;
        DataTransakcji = DateTime.Now;
        Komentarz = komentarz;
    }
    public Kierunek Kierunek { get; set; }
    public int AktywoId { get; set; }
    [Required]
    public virtual Aktywo Aktywo { get; set; }
    public int? PortfelId { get; set; }
    public Portfel Portfel { get; set; }
    public decimal Cena { get; set; }
    [Range(1, uint.MaxValue)]
    public uint Ilosc { get; set; }
    [Display(Name = "Data utworzenia transakcji")]
    public DateTime DataTransakcji { get; set; }
    public string Komentarz { get; set; } 
}



public class KupAktywoRequest
{
    public int? PortfelId { get; set; }
    public int AktywoId { get; set; }
    public DateTime DataTransakcji { get; set; }
    public Kierunek Kierunek { get; set; }
    [RegularExpression(@"^[0-9]+(\,[0-9]{1,2})$", ErrorMessage = "Wpisz kwotę z dokładnością do 2 miejsc po przecinku.")]
    public string Cena { get; set; }
    public uint Ilosc { get; set; }
    public string Komentarz { get; set; } 
    public bool Aktywna { get; set; } = true;
}
public class SprzedajAktywoRequest
{
    public int? PortfelId { get; set; }
    public int AktywoId { get; set; }
    public DateTime DataTransakcji { get; set; }
    public Kierunek Kierunek { get; set; }
    [RegularExpression(@"^[0-9]+(\,[0-9]{1,2})$", ErrorMessage = "Wpisz kwotę z dokładnością do 2 miejsc po przecinku.")]
    public string Cena { get; set; }
    public uint Ilosc { get; set; }
    public string Komentarz { get; set; } 
    public bool Aktywna { get; set; } = true;
}