using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portfel.Data.Data
{
    public class Aktualnosc
    {
        public int Id { get; set; }

        [Display(Name = "Pozycja")]
        public string Pozycja { get; set; }

        [Required(ErrorMessage = "Tytuł nagłówka jest wymagany")]
        [MinLength(5, ErrorMessage = "Tytuł artykułu powinien mieć minimum 5 znaków")]
        [Display(Name = "Tytuł nagłówka")]
        public string Tytul { get; set; }

        [Required(ErrorMessage = "Treść artykułu jest wymagana")]
        [MinLength(30, ErrorMessage = "Treść artykułu powinna mieć conajmniej 30 znaków")]
        [Column(TypeName = "nvarchar(MAX)")]
        [Display(Name = "Treść")]
        public string Tresc { get; set; }

        [Required(ErrorMessage = "Data dodania jest wymagana")]
        [Display(Name = "Data dodania")]
        public DateTime DataDodania { get; set; }
        public string FotoUrl { get; set; }
        public uint Priorytet { get; set; }
        [Display(Name = "Czy Aktywna")]
        public bool Aktywna { get; set; } = true;
    }
}
