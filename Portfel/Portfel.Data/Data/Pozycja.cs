namespace Portfel.Data.Data;

public class Pozycja : ObiektBazodanowy
{
    public Aktywo Aktywo { get; set; }
    public uint Ilosc { get; set; }
    public decimal SredniaCenaZakupu { get; set; }
}