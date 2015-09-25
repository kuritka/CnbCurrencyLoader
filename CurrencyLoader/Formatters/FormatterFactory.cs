using System;
using System.IO;
using CurrencyLoader.Formatters.Excel;

namespace CurrencyLoader.Formatters
{
    internal class FormatterFactory
    {
        private FileInfo _file;

        public FormatterFactory(FileInfo file)
        {
            _file = file;
        }

        public IFormatter Create()
        {
          
            if (_file.Extension == ".xls" || _file.Extension == ".xlsx")
            {
                return new ExcelFormatter(_file);
            }
            throw new NotImplementedException("Unsupported file type. Please use *.cs or *.xls");
        }
    }
}
