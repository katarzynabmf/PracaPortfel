using System.ComponentModel.DataAnnotations;

namespace Portfel.Data.Data
{
    public class Uzytkownik
    {
        [Key] //to niżej jest kluczem głównym, samo się automatycznie inkrementuje
        public int Id { get; set; }
        [Required]
        public string Imie { get; set; }
        [Required]
        public string Haslo { get; set; }
        [Required]
        public string Email { get; set; }

        //dla danego uzytkownika jest wiele kont
        public virtual ICollection<Konto> Konto { get; set; } = new List<Konto>();
    }
}
