using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portfel.Data.Data
{
    public class RodzajTransakcji
    {
        [Key] //to niżej jest kluczem głównym, samo się automatycznie inkrementuje
        public int Id { get; set; }
        [Required]
        public string Nazwa { get; set; }

        //danego rodzaju jest wiele transakcji
        public virtual ICollection<Transakcja> Transakcja { get; set; } = new List<Transakcja>();
    }
}
