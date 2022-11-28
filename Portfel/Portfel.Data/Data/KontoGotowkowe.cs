using System.ComponentModel.DataAnnotations;

namespace Portfel.Data.Data;

public class KontoGotowkowe : ObiektBazodanowy
{
    public Portfel Portfel { get; set; }
    public decimal StanKonta { get; set; } = 0;
    public ICollection<OperacjaGotowkowa> OperacjeGotowkowe { get; set; } = new List<OperacjaGotowkowa>();
}