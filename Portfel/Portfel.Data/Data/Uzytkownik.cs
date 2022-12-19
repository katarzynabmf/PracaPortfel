using System.ComponentModel.DataAnnotations;

namespace Portfel.Data.Data
{
    public class Uzytkownik
    {
        [Key] //to niżej jest kluczem głównym, samo się automatycznie inkrementuje
        public int Id { get; set; }
        [Required]
        [Display(Name = "Imię")]
        public string Imie { get; set; }
        [Required]

        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)\\S{4,20}$", ErrorMessage = "Hasło musi zawierać co najmniej jedną: dużą i małą literę, cyfrę oraz mieć długość 4-20 znaków")]

        public string Haslo { get; set; }
        [Required]
      
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{1,6}$", ErrorMessage = "Nieprawidłowy adres Email")]

        public string Email { get; set; }
        [Display(Name = "Czy Aktywny")]
        public bool Aktywna { get; set; } = true;
        public DateTime DataUtworzenia { get; set; } = DateTime.Now;
        public virtual ICollection<Portfel> Portfele { get; set; } = new List<Portfel>();
    }
}
