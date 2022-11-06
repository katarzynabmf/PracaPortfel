using Portfel.Data.Data;

namespace Portfel.App.Models
{
    public class SzczegolyKonta
    {
        public SzczegolyKonta(int idKonta, IEnumerable<Transakcja> transakcje)
        {
            IdKonta = idKonta;
            Transakcje = transakcje;
        }

        public int IdKonta { get; set; }
        public IEnumerable<Transakcja> Transakcje { get; set; }
    }
}
