using Portfel.Data.Data;

namespace Portfel.App.Models
{
    public class WszystkieTransakcjeDlaPortfela
    {
        public WszystkieTransakcjeDlaPortfela(int idPortfela, string nazwaPortfela, IEnumerable<TransakcjaNew> transakcje, int idKontaGotowkowego, IEnumerable<OperacjaGotowkowa> operacjeGotowkowe)
        {
            IdPortfela = idPortfela;
            NazwaPortfela = nazwaPortfela;
            Transakcje = transakcje;
            IdKontaGotowkowego = idKontaGotowkowego;
            OperacjeGotowkowe = operacjeGotowkowe;
           
        }
        public int IdPortfela { get; set; }
        public string NazwaPortfela { get; set; }

        public IEnumerable<TransakcjaNew> Transakcje { get; set; }
        public int IdKontaGotowkowego { get; set; }
        public IEnumerable<OperacjaGotowkowa> OperacjeGotowkowe { get; set; }
    }
}
