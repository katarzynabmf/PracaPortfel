using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portfel.Data.Data
{
    public class StworzAktualnoscRequest
    {

        public string Pozycja { get; set; }
        public string Tytul { get; set; }

        public string Tresc { get; set; }
        public DateTime DataDodania { get; set; }
        public string FotoUrl { get; set; }
        public uint Priorytet { get; set; }
        public bool Aktywna { get; set; } = true;
    }
}
