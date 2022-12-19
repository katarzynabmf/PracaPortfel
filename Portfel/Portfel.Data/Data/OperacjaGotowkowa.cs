namespace Portfel.Data.Data;

public class OperacjaGotowkowa : ObiektBazodanowy
{
    public OperacjaGotowkowa()
    {
            
    }
    public OperacjaGotowkowa(TypOperacjiGotowkowej typOperacjiGotowkowej, decimal kwota,
        KontoGotowkowe kontoGotowkowe)
    {
        Kwota = kwota;
        TypOperacjiGotowkowej = typOperacjiGotowkowej;
        DataOperacji = DateTime.Now;
        KontoGotowkowe = kontoGotowkowe;
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