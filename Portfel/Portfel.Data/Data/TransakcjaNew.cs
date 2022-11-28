namespace Portfel.Data.Data;

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
    public int? PortfelId { get; set; }
    public Portfel Portfel { get; set; }
    public decimal Cena { get; set; }
    public uint Ilosc { get; }
    public DateTime DataTransakcji { get; set; }
    public string Komentarz { get; set; } = "";
}