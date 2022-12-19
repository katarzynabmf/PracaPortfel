using Portfel.App.Controllers;
using Portfel.Data.Data;

namespace Portfel.App.Models
{
    public class SzczegolyPortfela
    {
        public SzczegolyPortfela(int idPortfela, string nazwaPortfela, IEnumerable<PozycjaSzczegoly> szczegolyPozycji)
        {
            IdPortfela = idPortfela;
            NazwaPortfela = nazwaPortfela;
            Pozycje = szczegolyPozycji;
        }

        public int IdPortfela { get; set; }
        public string NazwaPortfela { get; set; }
        public IEnumerable<PozycjaSzczegoly> Pozycje { get; set; }

    }
}
