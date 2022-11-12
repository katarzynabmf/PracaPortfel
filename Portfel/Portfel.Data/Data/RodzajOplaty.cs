using System.ComponentModel.DataAnnotations;

namespace Portfel.Data.Data
{
    public class RodzajOplaty
    {
        [Key] //to niżej jest kluczem głównym, samo się automatycznie inkrementuje
        public int Id { get; set; }
        [Required]
        public string Nazwa { get; set; }
        public bool Aktywna { get; set; } = true;
        public bool Promowany { get; set; }

        //danego rodzaju jest wiele oplat
        public virtual ICollection<Transakcja> Transakcja { get; set; } = new List<Transakcja>();
    }
}
