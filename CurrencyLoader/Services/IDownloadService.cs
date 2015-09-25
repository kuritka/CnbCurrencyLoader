using System;

namespace CurrencyLoader.Services
{
    public interface IDownloadService
    {
        string Download(DateTime forSpecifiedDate);
    }
}
