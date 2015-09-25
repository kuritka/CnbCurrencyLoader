using System;
using System.Collections.Generic;
using CurrencyLoader.Entities;

namespace CurrencyLoader.Services
{
    public interface IParser
    {
        List<CurrencyRate> Parse(string data);
    }
}
