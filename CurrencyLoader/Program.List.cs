using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data.SqlClient;
using CurrencyLoader.Entities;
using CurrencyLoader.Services.Cnb;

namespace CurrencyLoader
{
    partial class Program
    {
        private static void List()
        {
            ListCurrencies();            
            ShowDatabaseSettings();
        }

        private static void ShowDatabaseSettings()
        {
            Console.ForegroundColor = ConsoleColor.White;
            var builder = new SqlConnectionStringBuilder(Settings.Default.connectionString);
            builder.Password = "********";
            Console.WriteLine("db: {0}", builder);
            Console.WriteLine("web: {0}", Settings.Default.sourceCurrent);
        }

        private static void ListCurrencies()
        {            
            var currentData = Service.Get(DateTime.Now);
            using (var iterator = currentData.GetEnumerator())
            {
                while (iterator.MoveNext())
                {
                    var y = string.Empty;
                    var z = string.Empty;
                    var x = Format(iterator);
                    if (iterator.MoveNext())
                    {
                        y = Format(iterator);
                    }
                    if (iterator.MoveNext())
                    {
                        z = Format(iterator);
                    }
                    Console.WriteLine("{0}\t{1}\t{2}", x, y, z);
                }
            }
            Console.WriteLine();
        }

        private static string Format(List<CurrencyRate>.Enumerator iterator)
        {
            var rate = iterator.Current;
            return rate.AsString();
        }
    }
}
