using Portfel.Data.Data;

namespace Portfel.App.Models
{
    public class SzczegolyKonta
    {
        public SzczegolyKonta(int idKonta, string nazwaKonta, IEnumerable<Transakcja> transakcje)
        {
            IdKonta = idKonta;
            NazwaKonta = nazwaKonta;
            Transakcje = transakcje;
        }

        public int IdKonta { get; set; }
        public string NazwaKonta { get; set; }
        public IEnumerable<Transakcja> Transakcje { get; set; }
    }
}
