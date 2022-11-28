using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Portfel.Data.Data;

namespace Portfel.Data.Serwisy
{
    public class SymboleSerwis
    {
        private readonly PortfelContext _context;

        public SymboleSerwis(PortfelContext context)
        {
            _context = context;
        }

        public async Task ZaktualizujCeny()
        {
            var httpClient = new HttpClient();
            var aktywa = _context.Aktywa.ToList();
            foreach (var aktywo in aktywa)
            {
                var previousWorkDay = PreviousWorkDay(DateTime.Now).ToString("yyyy-MM-dd");
                var result = await httpClient.GetFromJsonAsync<PolygonWynik>(
                    @"https://api.polygon.io/v1/open-close/" + aktywo.Symbol + $"/{previousWorkDay}?adjusted=true&apiKey=TYEeIOppC_htdRTJpuIUIWgTGPYXbGhf");
                aktywo.CenaAktualna = (decimal) result.close;
                _context.Aktywa.Update(aktywo);
            }

            await _context.SaveChangesAsync();
        }

        private static DateTime PreviousWorkDay(DateTime date)
        {
            do
            {
                date = date.AddDays(-1);
            }
            while (IsWeekend(date));

            return date;
        }

        private static bool IsWeekend(DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Saturday ||
                   date.DayOfWeek == DayOfWeek.Sunday;
        }

        private class PolygonWynik
        {
            public string status { get; set; }
            public string from { get; set; }
            public string symbol { get; set; }
            public double open { get; set; }
            public double high { get; set; }
            public double low { get; set; }
            public double close { get; set; }
            public double volume { get; set; }
            public double afterHours { get; set; }
            public double preMarket { get; set; }
        }
    }
}
