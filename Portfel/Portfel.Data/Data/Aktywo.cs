using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Portfel.Data.Data;

public class Aktywo : ObiektBazodanowy
{
    public string Symbol { get; set; }
    public string Nazwa { get; set; }

    [Range(0, 9999999999.99)]
    public decimal CenaAktualna { get; set; }
}

public class AktywoViewModel
{
    public int Id { get; set; }
    public bool Aktywna { get; set; }
    public string Symbol { get; set; }
    public string Nazwa { get; set; }
    [Display(Name = "Aktualna cena")]

    [RegularExpression(@"^[0-9]+(\,[0-9]{1,2})$", ErrorMessage = "Valid Decimal number with maximum 2 decimal places.")]
    public string CenaAktualna { get; set; }
}