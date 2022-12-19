using System.ComponentModel.DataAnnotations;

namespace Portfel.Data.Data;

public class Pozycja : ObiektBazodanowy
{
    [Required]
    public Aktywo Aktywo { get; set; }
    [Range(1, uint.MaxValue)]
    public uint Ilosc { get; set; }
    public decimal SredniaCenaZakupu { get; set; }
}