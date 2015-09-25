using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CurrencyLoader.Entities;
using CurrencyLoader.Infrastructure;

namespace CurrencyLoader.Services.Cnb
{
    public static class CnbExtensions
    {
        public static Maybe<CurrencyRate> ToRate(this string text, DateTime dt)
        {
            if(string.IsNullOrEmpty(text)) return new Maybe<CurrencyRate>();
            var elements = text.Split('|');
            return new Maybe<CurrencyRate>(new CurrencyRate(
                elements[0], 
                elements[1],
                Int32.Parse(elements[2], CultureInfo.InvariantCulture), 
                elements[3],
                Decimal.Parse(elements[4], NumberStyles.AllowDecimalPoint), dt));
        }



        public static string AsString(this CurrencyRate rate)
        {
            if(rate == null) throw new ArgumentNullException("rate");
            return string.Format("{0,3} {1,4} {2,7}", rate.Currency, rate.Quantity, rate.Quotation);
        }



        public static bool IsNotNullOrEmpty<T>(this IEnumerable<T> collection)
        {
            return collection != null && collection.Any();
        }

    }
}
