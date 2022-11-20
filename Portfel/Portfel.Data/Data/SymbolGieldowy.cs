using System.ComponentModel.DataAnnotations;

namespace Portfel.Data.Data
{
    public class SymbolGieldowy
    {
        [Key] //to niżej jest kluczem głównym, samo się automatycznie inkrementuje
        public int Id { get; set; }
        [Required]
        public string Nazwa { get; set; }
        [Display(Name = "Czy Aktywny")]
        public bool Aktywna { get; set; } = true;
        //danego symbolu jest wiele transakcji
        public virtual ICollection<Transakcja> Transakcja { get; set; } = new List<Transakcja>();
    }
}
