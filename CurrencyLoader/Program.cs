using System;
using CommandLine;
using CurrencyLoader.Entities;
using CurrencyLoader.Services.Cnb;

namespace CurrencyLoader
{
    public static partial class Program
    {
        private static readonly CnbService Service = new CnbService(new CnbDownloadService(), new CnbParser());


        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            try
            {
                var options = new CommandLineOptions();
                if (Parser.Default.ParseArguments(args, options))
                {
                    if (options.List)
                    {
                        List();
                        return;
                    }
                    if (!options.HasFromDate)
                    {
                        options.FromDate = DateTime.Now.Date;
                    }
                  
                    Download(options.FromDate, options.Currency, options);
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("{0}{0}ERROR", Environment.NewLine);
                Console.WriteLine("Name: {0}", ex.GetType().Name);
                Console.WriteLine("Message: {0}", ex.Message);
                if (ex.InnerException != null)
                    Console.WriteLine("Inner Message: {0}", ex.InnerException.Message);
            }
            finally
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("{0}use -h parameter for more options{0}",Environment.NewLine);
            }
        }

        
    }
}
