using System;
using CurrencyLoader.Formatters.Excel;

namespace CurrencyLoader.Entities
{
    public class CurrencyRate
    {

        public CurrencyRate(string country, string ccyName, int quantity, string currency, decimal quotation, DateTime date)
        {
            if (string.IsNullOrEmpty(country)) throw new ArgumentException("country");
            if (string.IsNullOrEmpty(ccyName)) throw new ArgumentException("ccyName");
            if (quantity <= 0 && quantity > 10000) throw new ArgumentException("quantity is <= 0");
            if (string.IsNullOrEmpty(currency) || currency.Length != 3) throw new ArgumentException("currency");
            if (quotation <= 0 && quotation > 100) throw new ArgumentException("quotation");
            Country = country;
            CcyName = ccyName;
            Quantity = quantity;
            Currency = currency;
            Quotation = quotation;
            Date = date;
        }

        public DateTime Date { get; private set; }

        public string Currency { get; private set; }

        public string Country { get; private set; }

        public string CcyName { get; private set; }

        public int Quantity { get; private set; }
        
        public decimal Quotation { get; private set; }
    }


    
    //todo: Repair ExcelFormatter that cannot process floating point datatypes. after that this class can be deleted
    public class ExcelRate
    {
        private CurrencyRate _currency;

        public ExcelRate(CurrencyRate currency)
        {
            _currency = currency;
        }


        [Column("Date", 10)]
        public DateTime Date { get { return _currency.Date; }}

        [Column("Code", 10)]
        public string Currency { get { return _currency.Currency; } }

        [Column("Country", 20)]
        public string Country { get { return _currency.Country; } }

        [Column("Currency", 15)]
        public string CcyName { get { return _currency.CcyName; } }

        [Column("Quantity", 10)]
        public int Quantity { get { return _currency.Quantity; } }

        [Column("Rate", 10)]
        public string  Quotation { get { return string.Format("{0}",_currency.Quotation); } }
    }
}
