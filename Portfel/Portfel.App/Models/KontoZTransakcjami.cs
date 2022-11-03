using Portfel.Data.Data;

namespace Portfel.App.Models
{
    public class KontoZTransakcjami
    {
        public int? Konto { get; set; }
        public IEnumerable<Transakcja> Transakcje { get; set; }
    }
}
