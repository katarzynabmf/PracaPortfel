using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Portfel.Data.Data
{
    public class Konto
    {
        [Key] //to niżej jest kluczem głównym, samo się automatycznie inkrementuje
        public int Id { get; set; }
        [Required]
        public string Nazwa { get; set; }
        [Required]
        public string Waluta { get; set; }
        public double Gotowka { get; set; }

        public int? UzytkownikId { get; set; }
        public virtual Uzytkownik Uzytkownik { get; set; }
    }
}
