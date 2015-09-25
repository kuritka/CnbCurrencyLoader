using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CurrencyLoader.Entities;

namespace CurrencyLoader.Services.Cnb
{
    public class CnbParser : IParser
    {
        public List<CurrencyRate> Parse(string data)
        {
            var currencyRates = new List<CurrencyRate>();
            if (string.IsNullOrEmpty(data)) throw new ArgumentException("data");
            try
            {
                var date = DateTime.ParseExact(data.Substring(0, 10), "dd.MM.yyyy", null,
                    DateTimeStyles.None);
                var splitted = data.Split('\r', '\n');
                if (!splitted[1].StartsWith("země|měna|množství|kód|kurz",true,CultureInfo.InvariantCulture))
                {
                    throw new InvalidDataException("Second line doesnt start with země|měna|množství|kód|kurz");
                }
                var clearData = splitted.Skip(2).ToList();
                
                currencyRates.AddRange(from line in clearData select line.ToRate(date) into x where x.Any() select x.Single());
                return currencyRates;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error during parsing phase...",ex);
            }
        }


    }
}
