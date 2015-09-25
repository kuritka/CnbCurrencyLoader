using System.Collections.Generic;
using CurrencyLoader.Entities;

namespace CurrencyLoader.Formatters
{
    interface IFormatter
    {
        void Format(IEnumerable<ExcelRate> currencies);
    }
}
