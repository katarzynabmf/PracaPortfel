using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


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
        [Display(Name = "Użytkownik")]
        public int? UzytkownikId { get; set; }
        public Uzytkownik Uzytkownik { get; set; }
        [Display(Name = "Czy Aktywne")]
        public bool Aktywna { get; set; } = true;

        public double SumaNaKoncie
        {
            get
            {
                return Gotowka + Transakcje.Sum(transakcja => transakcja.Ilosc * transakcja.Kwota);
            }
        }

        [InverseProperty("Konto")]
        public ICollection<Transakcja> Transakcje { get; set; }
    }

    public class EdytujKontoRequest
    {
        public int Id { get; set; }
        public string Nazwa { get; set; }
        public string Waluta { get; set; }
        public double Gotowka { get; set; }
        public int? UzytkownikId { get; set; }
        public bool Aktywna { get; set; }
    }
}
