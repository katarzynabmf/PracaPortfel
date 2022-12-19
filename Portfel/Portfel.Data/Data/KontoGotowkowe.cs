using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portfel.Data.Data;

public class KontoGotowkowe : ObiektBazodanowy
{
    
  
    public decimal StanKonta { get; set; }
    public int PortfelId { get; set; }
    public Portfel Portfel { get; set; }
    public ICollection<OperacjaGotowkowa> OperacjeGotowkowe { get; set; } = new List<OperacjaGotowkowa>();
}