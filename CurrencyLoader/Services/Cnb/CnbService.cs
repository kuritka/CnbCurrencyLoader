using System;
using System.Collections.Generic;
using CurrencyLoader.DataAccess;
using CurrencyLoader.Entities;
using CurrencyLoader.Infrastructure;

namespace CurrencyLoader.Services.Cnb
{

    public interface ICnbService 
    {
        List<CurrencyRate> Get(DateTime from);
    }


    public class CnbService : ICnbService
    {
        private readonly IDownloadService _downloadService;
        private readonly IParser _parser;

        public CnbService(IDownloadService downloadService, IParser parser)
        {
            _downloadService = downloadService;
            _parser = parser;
        }

     
        public List<CurrencyRate> Get(DateTime @from)
        {
            if (from < Constants.MinDate) throw new ArgumentException("fromDate");
            if (from >= DateTime.Now.Date.AddDays(1)) throw new ArgumentException("fromDate");
            var data = _downloadService.Download(from);
            return _parser.Parse(data);
        }





        public void Upsert(IEnumerable<CurrencyRate> rates)
        {
            dynamic sql = new DynamicSqlOrm();
            foreach (var rate in rates)
            {
                sql.CurrencyTableUpsert(rate, Date: rate.Date, Currency: rate.Currency);    
            }
        }

    }
}
