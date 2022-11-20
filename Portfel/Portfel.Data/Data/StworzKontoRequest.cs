namespace Portfel.Data.Data;

public class StworzKontoRequest
{
    public string Nazwa { get; set; }
    public string Waluta { get; set; }
    public double Gotowka { get; set; }

    public int? UzytkownikId { get; set; }
    public bool Aktywna { get; set; }
}