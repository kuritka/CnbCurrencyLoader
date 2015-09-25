using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using CurrencyLoader.Entities;
using CurrencyLoader.Formatters;
using CurrencyLoader.Infrastructure;
using CurrencyLoader.Services.Cnb;

namespace CurrencyLoader
{
    partial class Program
    {
        private static void Download(DateTime fromDate, IList<string> currencies, CommandLineOptions options)
        {
            var totalResult = new List<CurrencyRate>((int) ((DateTime.Now.Date - options.FromDate.Date).TotalDays * 50) );
            Console.WriteLine("Downloading {1} from {0}... ", options.FromDate.ToString("dd.MM.yyyy"), string.Join(",", currencies));
            Console.ForegroundColor = ConsoleColor.White;
            foreach (var data in LoadData(fromDate, currencies))
            {
                totalResult.AddRange(data);
                Console.Write("-");
            }
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("{0}{0}Writing {1} records to database...", Environment.NewLine, totalResult.Count);
            Flush(options, totalResult);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("{0}Done.", Environment.NewLine);
        }

        private static void Flush(CommandLineOptions options, IEnumerable<CurrencyRate> totalResult)
        {
            if (options.HasOutputFile)
            {
                new FormatterFactory(new FileInfo(options.OutputFile)).Create().Format(totalResult.Select(d=>new ExcelRate(d)));
            }
            else
            {
                Service.Upsert(totalResult);
            }
        }


        private static IEnumerable<List<CurrencyRate>> LoadData(DateTime fromDate, IList<string> currencies)
        {
            if (fromDate.Date > DateTime.Now.Date) throw new ArgumentException("fromDate");
            if (fromDate.Date < Constants.MinDate) throw new ArgumentException("fromDate is too small");
            var dates = Enumerable.Range(0, (DateTime.Now.Date - fromDate.Date).Days + 1)
                        .Select(d => fromDate.AddDays(d));
            foreach (var from in dates)
            {
                var currenciesForDay = Service.Get(from);
                if (currencies.IsNotNullOrEmpty())
                {
                    currenciesForDay = currenciesForDay
                        .Where(d => currencies.Contains(d.Currency))
                        .ToList();
                }
                Thread.Sleep(250);
                yield return currenciesForDay;
            }
            
        }
       

    }
}
