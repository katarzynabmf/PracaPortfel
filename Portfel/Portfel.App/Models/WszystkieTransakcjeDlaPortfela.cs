using Portfel.Data.Data;

namespace Portfel.App.Models
{
    public class WszystkieTransakcjeDlaPortfela
    {
        public WszystkieTransakcjeDlaPortfela(int idPortfela, string nazwaPortfela, IEnumerable<TransakcjaNew> transakcje)
        {
            IdPortfela = idPortfela;
            NazwaPortfela = nazwaPortfela;
            Transakcje = transakcje;
        }
        public int IdPortfela { get; set; }
        public string NazwaPortfela { get; set; }

        public IEnumerable<TransakcjaNew> Transakcje { get; set; }
    }
}
