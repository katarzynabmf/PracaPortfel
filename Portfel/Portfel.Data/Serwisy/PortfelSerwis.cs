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
            portfel.KontoGotowkowe.OperacjeGotowkowe.Add(operacjaGotowkowa);
            _context.SaveChanges();
            operacjaGotowkowa.Wykonaj();
            _context.Portfele.Update(portfel);
            _context.SaveChanges();
        }
        public void WyplacSrodkiZKonta(decimal kwota, int portfelId)
        {
            var portfel = _context.Portfele
                .Include(p => p.KontoGotowkowe)
                .FirstOrDefault(p => p.Id == portfelId);
            WyplacSrodkiZKonta(kwota, portfel);
        }
        public void WyplacSrodkiZKonta(decimal kwota, Data.Portfel portfel)
        {
            if (kwota <= 0)
            {
                throw new ArgumentException("Nie można wprowadzić ujemniej lub równej 0 kwoty");
            }
            var operacjaGotowkowa = new OperacjaGotowkowa(TypOperacjiGotowkowej.Wyplata, kwota, portfel.KontoGotowkowe);
            portfel.KontoGotowkowe.OperacjeGotowkowe.Add(operacjaGotowkowa);
            _context.SaveChanges();
            operacjaGotowkowa.Wykonaj();
            _context.Portfele.Update(portfel);
            _context.SaveChanges();
        }
        public void KupAktywo(string symbolAktywa, uint ilosc, decimal cena, int portfelId, string komentarz)
        {
            var portfel = _context.Portfele
                .Include(p => p.KontoGotowkowe)
                .Include(p=>p.Pozycje)
                .ThenInclude(p => p.Aktywo)
                .FirstOrDefault(p => p.Id == portfelId);
            ZawrzyjTransakcje(symbolAktywa, ilosc, cena, Kierunek.Kupno, komentarz, portfel);
        }

        public void KupAktywo(string symbolAktywa, uint ilosc, decimal cena, string komentarz, Data.Portfel portfel)
        {
            ZawrzyjTransakcje(symbolAktywa, ilosc, cena, Kierunek.Kupno, komentarz, portfel);
        }
        public void SprzedajAktywo(string symbolAktywa, uint ilosc, decimal cena,  int portfelId, string komentarz)
        {
            var portfel = _context.Portfele
                .Include(p => p.KontoGotowkowe)
                .Include(p => p.Pozycje)
                .ThenInclude(p => p.Aktywo)
                .FirstOrDefault(p => p.Id == portfelId);
            ZawrzyjTransakcje(symbolAktywa, ilosc, cena, Kierunek.Sprzedaz, komentarz, portfel);
        }
        public void SprzedajAktywo(string symbolAktywa, uint ilosc, decimal cena,string komentarz, Data.Portfel portfel)
        {
            ZawrzyjTransakcje(symbolAktywa, ilosc, cena, Kierunek.Sprzedaz, komentarz, portfel);
        }

        private void ZawrzyjTransakcje(string symbolAktywa, uint ilosc, decimal cena, Kierunek kierunek,string komentarz, Data.Portfel portfel)
        {
            if (cena <= 0)
            {
                throw new ArgumentException("Nie można wprowadzić ujemniej lub równej 0 ceny");
            }

            //policz stan portfela po operacji - bez zapisu do BD
            var aktywoTransakcji = _context.Aktywa.Single(s => s.Symbol == symbolAktywa);
            //var transakcjaPolicz = _context.TransakcjeNew.Add(new TransakcjaNew(aktywoPolicz, kierunek, cena, ilosc));
            //portfel.Transakcje.Add(transakcjaPolicz.Entity);
            var kwotaWymaganaDoZawarciaTransakcji = cena * ilosc;

            if (kwotaWymaganaDoZawarciaTransakcji > portfel.KontoGotowkowe.StanKonta && kierunek == Kierunek.Kupno)
            {
                throw new InvalidOperationException("Brak wystarczających środków na koncie.");
            }

            //var nowaPozycjaPolicz = new Pozycja() { Aktywo = aktywoTransakcji, Ilosc = ilosc, SredniaCenaZakupu = cena };
            //var posiadanaIloscJednostekDanegoAktywa =
            //    portfel.Pozycje.SingleOrDefault(p => p.Aktywo.Symbol == symbolAktywa)?.Ilosc ?? 0;

            if (kierunek == Kierunek.Sprzedaz)
            {
                var posiadanaIloscJednostekDanegoAktywa =
                    portfel.Pozycje.SingleOrDefault(p => p.Aktywo.Symbol == symbolAktywa)?.Ilosc ?? 0;

                if (posiadanaIloscJednostekDanegoAktywa < ilosc)
                {
                   throw new InvalidOperationException("Brak wystarczającej ilości aktywa.");
                }
            }


            //zapisz stan portfela po operacji

            var aktywo = _context.Aktywa.Single(s => s.Symbol == symbolAktywa);
            //var transakcja = _context.TransakcjeNew.Add(new TransakcjaNew(aktywo, kierunek, cena, ilosc));
            portfel.Transakcje.Add(new TransakcjaNew(aktywo, kierunek, cena, ilosc, komentarz));
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
        void WyplacSrodkiZKonta(decimal kwota, int portfelId);
        void KupAktywo(string symbolAktywa, uint ilosc, decimal cena, int portfelId, string komenatrz);
        void SprzedajAktywo(string symbolAktywa, uint ilosc, decimal cena, int portfelId, string komenatrz);
    }
 
}
