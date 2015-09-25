using System.Collections.Generic;
using System.IO;
using CurrencyLoader.Entities;

namespace CurrencyLoader.Formatters.Excel
{
    internal class ExcelFormatter : IFormatter
    {
        private readonly FileInfo _file;

        public ExcelFormatter(FileInfo file)
        {
            _file = file;
        }

        public void Format(IEnumerable<ExcelRate> currencies)
        {                        
            var excel = ExcelProvider.GetExcel(ExcelType.CobaWithLogo, currencies, "Currency Rates");
            File.WriteAllBytes(_file.FullName, excel.ToArray());                     
        }
    }
}
