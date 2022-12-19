using Portfel.Data.Data;

namespace Portfel.App.Models
{
    public class AktualnosciIAktywa
    {
        public AktualnosciIAktywa(IEnumerable<Aktualnosc> aktualnosci, IEnumerable<Aktywo> aktywa)
        {
            Aktualnosci = aktualnosci;
            Aktywa = aktywa;
        }
        public IEnumerable<Aktywo> Aktywa;
        public IEnumerable<Aktualnosc> Aktualnosci;
    }
}
