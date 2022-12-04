using Portfel.Data.Data;

namespace Portfel.App.Models
{
    public class SzczegolyPortfela
    {
        public SzczegolyPortfela(int idPortfela, string nazwaPortfela, IEnumerable<Pozycja> pozycje)
        {
            IdPortfela = idPortfela;
            NazwaPortfela = nazwaPortfela;
            Pozycje = pozycje;
        }
        public int IdPortfela { get; set; }
        public string NazwaPortfela { get; set; }

        public IEnumerable<Pozycja> Pozycje { get; set; }

    }
}
