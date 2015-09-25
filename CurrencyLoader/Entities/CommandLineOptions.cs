using System;
using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;
using CurrencyLoader.Services.Cnb;

namespace CurrencyLoader.Entities
{
    internal class CommandLineOptions
    {
        [OptionList('c', "currency", Required = false, HelpText = "Currency you want to download i.e. USD,EUR,CNY. If you dont specify currency, all possible currencies are download", Separator = ',', DefaultValue = new string[]{})]
        public IList<string> Currency { get;set; }
        
        [Option('o', "output", Required = false, HelpText = "Database or excel. Database is default ")]
        public string OutputFile { get; set; }


        [Option('l', "list", Required = false, HelpText = "List all possible currencies and shows possible database to update")]
        public bool List { get; set; }

        [Option('f', "fromDate", Required = false, HelpText = "From specified date (MM-DD-YYYY). If parameter is not setted than today is is applied.")]
        public DateTime FromDate { get; set; }


        public bool HasOutputFile
        {
            get { return !string.IsNullOrEmpty(OutputFile); }
        }


        public bool HasFromDate
        {
            get { return !(FromDate == DateTime.MinValue); }
        }

        public bool HasCurrencies
        {
            get { return !Currency.IsNotNullOrEmpty(); }
        }


        [HelpOption]
        public string GetUsage()
        {
            return string.Format("{0}{1}Example of use:{1}{1}" +
                                 " CurrencyLoader.exe -c USD,EUR,HUF -f 04-01-2015{1}  + gets specified currencies from 01. April 2015 and save them to database{1}{1}" +
                                 " CurrencyLoader.exe {1}  + gets all currencies for actual date{1}{1}" +
                                 " CurrencyLoader.exe -f 06-01-2015 -c USD,HUF,CNY -o C:\\xyz.xlsx {1}  + gets specified rates from 01 June 2015 and save them to excel{1}{1}" +
                                 " CurrencyLoader.exe -l {1}  + shows actual currencies to console{1}{1}",
                
                HelpText.AutoBuild(this, current => HelpText.DefaultParsingErrorsHandler(this, current)),
                Environment.NewLine);
        }


    }
}
