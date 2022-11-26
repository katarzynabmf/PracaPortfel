using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portfel.Data.Data;

namespace Portfel.Data.Serwisy
{
    public class PortfelSerwis
    {
        private readonly PortfelContexts _context;

        public PortfelSerwis(PortfelContexts context)
        {
            _context = context;
        }

        public void WplacSrodkiNaKonto(decimal kwota, Data.Portfel portfel)
        {
            var operacjaGotowkowa = new OperacjaGotowkowa(TypOperacjiGotowkowej.Wplata, kwota);
            portfel.KontoGotowkowe.OperacjeGotowkowe.Add(operacjaGotowkowa);
            _context.SaveChanges();
            operacjaGotowkowa.Wykonaj();
            _context.Portfele.Update(portfel);
            _context.SaveChanges();
        }

        public void WyplacSrodkiZKonta(decimal kwota, Data.Portfel portfel)
        {
            var operacjaGotowkowa = new OperacjaGotowkowa(TypOperacjiGotowkowej.Wyplata, kwota);
            portfel.KontoGotowkowe.OperacjeGotowkowe.Add(operacjaGotowkowa);
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
            var aktywo = _context.Aktywa.Single(s => s.Symbol == symbolAktywa);
            var transakcja = _context.TransakcjeNew.Add(new TransakcjaNew(aktywo, kierunek, cena, ilosc));
            portfel.Transakcje.Add(transakcja.Entity);
            var kwotaTransakcji = cena * ilosc;

            var operacjaGotowkowa = new OperacjaGotowkowa(kierunek == Kierunek.Kupno ? TypOperacjiGotowkowej.Obciazenie : TypOperacjiGotowkowej.Uznanie, kwotaTransakcji);
            portfel.KontoGotowkowe.OperacjeGotowkowe.Add(operacjaGotowkowa);
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
                    throw new InvalidOperationException("Brak aktywow tego typu w systemie.");
                
                portfel.Pozycje.Add(nowaPozycja);
            }
            _context.SaveChanges();
        }
    }
}
