using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Portfel.Data.Data;

namespace Portfel.Data.Serwisy
{
    public class PortfelSerwis : IPortfelSerwis
    {
        private readonly PortfelContext _context;

        public PortfelSerwis(PortfelContext context)
        {
            _context = context;
        }

        public void WplacSrodkiNaKonto(decimal kwota, int portfelId)
        {
            //var portfel = _context.Portfele.
            //    Find(portfelId);
            var portfel = _context.Portfele
                .Include(p => p.KontoGotowkowe)
                .FirstOrDefault(p=>p.Id == portfelId);
            WplacSrodkiNaKonto(kwota, portfel);
        }

        public void WplacSrodkiNaKonto(decimal kwota, Data.Portfel portfel)
        {
            if (kwota <= 0)
            {
                throw new ArgumentException("Nie można wprowadzić ujemniej lub równej 0 kwoty");
            }

            var operacjaGotowkowa = new OperacjaGotowkowa(TypOperacjiGotowkowej.Wplata, kwota, portfel.KontoGotowkowe);
            //portfel.KontoGotowkowe.OperacjeGotowkowe.Add(operacjaGotowkowa);
            _context.SaveChanges();
            operacjaGotowkowa.Wykonaj();
            _context.Portfele.Update(portfel);
            _context.SaveChanges();
        }

        public void WyplacSrodkiZKonta(decimal kwota, Data.Portfel portfel)
        {
            if (kwota <= 0)
            {
                throw new ArgumentException("Nie można wprowadzić ujemniej lub równej 0 kwoty");
            }
            var operacjaGotowkowa = new OperacjaGotowkowa(TypOperacjiGotowkowej.Wyplata, kwota, portfel.KontoGotowkowe);
            //portfel.KontoGotowkowe.OperacjeGotowkowe.Add(operacjaGotowkowa);
            _context.SaveChanges();
            operacjaGotowkowa.Wykonaj();
            _context.Portfele.Update(portfel);
            _context.SaveChanges();
        }

        public void KupAktywo(string symbolAktywa, uint ilosc, decimal cena, Data.Portfel portfel)
        {
            ZawrzyjTransakcje(symbolAktywa, ilosc, cena, Kierunek.Kupno, portfel);
        }

        public void SprzedajAktywo(string symbolAktywa, uint ilosc, decimal cena, Data.Portfel portfel)
        {
            ZawrzyjTransakcje(symbolAktywa, ilosc, cena, Kierunek.Sprzedaz, portfel);

        }

        private void ZawrzyjTransakcje(string symbolAktywa, uint ilosc, decimal cena, Kierunek kierunek, Data.Portfel portfel)
        {
            if (cena <= 0)
            {
                throw new ArgumentException("Nie można wprowadzić ujemniej lub równej 0 ceny");
            }
       


            //policz stan portfela po operacji - bez zapisu do BD
            var aktywoPolicz = _context.Aktywa.Single(s => s.Symbol == symbolAktywa);
            var transakcjaPolicz = _context.TransakcjeNew.Add(new TransakcjaNew(aktywoPolicz, kierunek, cena, ilosc));
            portfel.Transakcje.Add(transakcjaPolicz.Entity);
            var kwotaTransakcjiPolicz = cena * ilosc;

            if (kwotaTransakcjiPolicz > portfel.KontoGotowkowe.StanKonta && kierunek == Kierunek.Kupno)
            {
                throw new InvalidOperationException("Brak wystarczających środków na koncie.");
            }

            var nowaPozycjaPolicz = new Pozycja() { Aktywo = aktywoPolicz, Ilosc = ilosc, SredniaCenaZakupu = cena };
            var istniejacaPozycjaPolicz = portfel.Pozycje.SingleOrDefault(p => p.Aktywo.Symbol == symbolAktywa);

            if (kierunek == Kierunek.Sprzedaz)
            {
                if (istniejacaPozycjaPolicz.Ilosc < nowaPozycjaPolicz.Ilosc )
                {
                   throw new InvalidOperationException("Brak wystarczającej ilości aktywa.");
                }
            }


            //zapisz stan portfela po operacji

            var aktywo = _context.Aktywa.Single(s => s.Symbol == symbolAktywa);
            var transakcja = _context.TransakcjeNew.Add(new TransakcjaNew(aktywo, kierunek, cena, ilosc));
            portfel.Transakcje.Add(transakcja.Entity);
            var kwotaTransakcji = cena * ilosc;

            var operacjaGotowkowa = new OperacjaGotowkowa(kierunek == Kierunek.Kupno ? TypOperacjiGotowkowej.Obciazenie : TypOperacjiGotowkowej.Uznanie, kwotaTransakcji, portfel.KontoGotowkowe);
            //portfel.KontoGotowkowe.OperacjeGotowkowe.Add(operacjaGotowkowa);
            _context.SaveChanges();
            operacjaGotowkowa.Wykonaj();
            _context.Portfele.Update(portfel);
            _context.SaveChanges();

            var nowaPozycja = new Pozycja(){Aktywo = aktywo, Ilosc = ilosc, SredniaCenaZakupu = cena};
            var istniejacaPozycja = portfel.Pozycje.SingleOrDefault(p => p.Aktywo.Symbol == symbolAktywa);
            if (istniejacaPozycja is not null)
            {
                var zmianaIlosci = nowaPozycja.Ilosc * (kierunek == Kierunek.Sprzedaz ? -1 : 1);
                if (istniejacaPozycja.Ilosc + zmianaIlosci < 0)
                    throw new InvalidOperationException();
                istniejacaPozycja.Ilosc += (uint) zmianaIlosci;
        
                if(kierunek == Kierunek.Kupno)
                    istniejacaPozycja.SredniaCenaZakupu = (istniejacaPozycja.SredniaCenaZakupu + nowaPozycja.SredniaCenaZakupu) / 2;
                if (istniejacaPozycja.Ilosc == 0)
                {
                    _context.Pozycje.Remove(istniejacaPozycja);
                }
            }
            else
            {
                if (kierunek == Kierunek.Sprzedaz)
                    throw new InvalidOperationException("Brak aktywów tego typu w systemie.");
                
                portfel.Pozycje.Add(nowaPozycja);
            }
            _context.SaveChanges();
        }
    }

    public interface IPortfelSerwis
    {
        void WplacSrodkiNaKonto(decimal kwota, int portfelId);
    }
}
