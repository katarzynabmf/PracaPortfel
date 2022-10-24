using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portfel.Data.Data
{
    public class Transakcja
    {
        [Key] //to niżej jest kluczem głównym, samo się automatycznie inkrementuje
        public int Id { get; set; }
        [Required]
        public int? KontoId { get; set; }
        public Konto Konto { get; set; }
        public DateTime Date { get; set; }

        [Required]
        public int? RodzajTransakcjiId { get; set; }
        public virtual RodzajTransakcji RodzajTransakcji { get; set; }
        [Required]
        public string Waluta { get; set; }

        public int? SymbolGieldowyId { get; set; }
        public virtual SymbolGieldowy SymbolGieldowy { get; set; }

        [Required]
        public double Kwota { get; set; }

        [Required]
        public int Ilosc { get; set; }

        public int? RodzajOplatyId { get; set; }
        public virtual RodzajOplaty RodzajOplaty { get; set; }
        public string Komentarz { get; set; }
    }
    public class StworzTransakcjaRequest
    {
 
        public int? KontoId { get; set; }
        public DateTime Date { get; set; }

        public int? RodzajTransakcjiId { get; set; }
  
        public string Waluta { get; set; }

        public int? SymbolGieldowyId { get; set; }
        public double Kwota { get; set; }
        public int Ilosc { get; set; }

        public int? RodzajOplatyId { get; set; }
        public string Komentarz { get; set; }
    }
    public class EdytujTransakcjaRequest
    {
        public int Id { get; set; }
        public int? KontoId { get; set; }
        public DateTime Date { get; set; }

        public int? RodzajTransakcjiId { get; set; }

        public string Waluta { get; set; }

        public int? SymbolGieldowyId { get; set; }
        public double Kwota { get; set; }
        public int Ilosc { get; set; }

        public int? RodzajOplatyId { get; set; }
        public string Komentarz { get; set; }
    }
}
