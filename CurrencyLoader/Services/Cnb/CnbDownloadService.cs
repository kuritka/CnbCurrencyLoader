using System;
using System.IO;
using System.Net;
using System.Security.Policy;
using CurrencyLoader.Infrastructure;

namespace CurrencyLoader.Services.Cnb
{
    public class CnbDownloadService : IDownloadService
    {

        public string Download(DateTime forSpecifiedDate)
        {
            if (forSpecifiedDate <= Constants.MinDate) throw new ArgumentException("date");
            if (forSpecifiedDate > DateTime.Now.Date.AddDays(1)) throw new ArgumentException("date");
            return Download(new Url(string.Format(Settings.Default.sourceDDMMYYYY, 
                forSpecifiedDate.Day, forSpecifiedDate.Month, forSpecifiedDate.Year)));
        }

        private string Download(Url url)        
        {
            string source;
            var request = WebRequest.Create(url.Value);
            using (var reader = new StreamReader(request.GetResponse().GetResponseStream()))
            {
                source = reader.ReadToEnd();
            }
            return source;
        }
    }
}
