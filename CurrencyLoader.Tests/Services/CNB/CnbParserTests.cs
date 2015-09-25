using System;
using CurrencyLoader.Services;
using CurrencyLoader.Services.Cnb;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using FluentAssertions;

namespace CurrencyLoader.Tests.Services.CNB
{
    [TestClass]
    public class CnbParserTests
    {
        private readonly IParser _testee;



        public CnbParserTests()
        {
            _testee = new CnbParser();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ParseNullText()
        {
            _testee.Parse(null);
           
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ParseStringEmptyText()
        {
            _testee.Parse("");
        }


        [TestMethod]
        public void ParseTextWithSpecialSymbols()
        {
            var input =
                "16.09.2015 #180\n" +
                "země|měna|množství|kód|kurz\n" +
                "Au\u00dftráli%|d*larí|1|AUD|17,258\n";
                
            var result = _testee.Parse(input);
            result.Count.ShouldBeEquivalentTo(1);
            result[0].Country.Should().Be("Au\u00dftráli%");
            result[0].CcyName.Should().Be("d*larí");

        }

        [TestMethod]
        public void ParseValidInput()
        {
            var input =
                "16.09.2015 #180\n" +
                "země|měna|množství|kód|kurz\n" +
                "Austrálie|dolar|1|AUD|17,258\n" +
                "Brazílie|real|1|BRL|6,270\n" +
                "Bulharsko|lev|1|BGN|13,835\n" +
                "Čína|renminbi|1|CNY|3,783\n" +
                "Dánsko|koruna|1|DKK|3,627\n" ;

            var result = _testee.Parse(input);
            result.Count.ShouldBeEquivalentTo(5);
            result[0].Country.Should().Be("Austrálie");
            result[1].CcyName.Should().Be("real");
            result[2].Currency.Should().Be("BGN");
            result[3].Quotation.ShouldBeEquivalentTo(3.783);
            result[4].Quantity.ShouldBeEquivalentTo(1);

        }


        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void ParseInvalidInputWithInvalidDate()
        {
            var input =
                "16.09.2 #180\n" +
                "země|měna|množství|kód|kurz\n" +
                "Austrálie|dolar|1|AUD|17,258\n" +
                "Brazílie|real|1|BRL|6,270\n" +
                "Bulharsko|lev|1|BGN|13,835\n" +
                "Čína|renminbi|1|CNY|3,783\n" +
                "Dánsko|koruna|1|DKK|3,627\n";

            _testee.Parse(input);
        }


        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void ParseInvalidInputWithoutDate()
        {
            var input =
                "země|měna|množství|kód|kurz\n" +
                "Austrálie|dolar|1|AUD|17,258\n" +
                "Brazílie|real|1|BRL|6,270\n" +
                "Bulharsko|lev|1|BGN|13,835\n" +
                "Čína|renminbi|1|CNY|3,783\n" +
                "Dánsko|koruna|1|DKK|3,627\n";

            _testee.Parse(input);

        }


        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void ParseInvalidInputWithoutColumnNames()
        {
            var input =
              "16.09.2015 #180\n" +
              "Austrálie|dolar|1|AUD|17,258\n" +
              "Brazílie|real|1|BRL|6,270\n" +
              "Bulharsko|lev|1|BGN|13,835\n" +
              "Čína|renminbi|1|CNY|3,783\n" +
              "Dánsko|koruna|1|DKK|3,627\n";

            _testee.Parse(input);
          
        }


        [TestMethod]
        public void ParseValidInputWithoutData()
        {
            var input =
                "16.09.2015 #180\n" +
                "země|měna|množství|kód|kurz\n";
            var result = _testee.Parse(input);
            result.Count.Should().Be(0);

        }




        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void ParseInvalidInputWithWrongRate()
        {
            var input =
                "16.09.2015 #180\n" +
                "země|měna|množství|kód|kurz\n" +
                "Austrálie|dolar|1|AUD|17,258x\n" +
                "Brazílie|real|1|BRL|6,270\n" +
                "Bulharsko|lev|1|BGN|13,835\n" +
                "Čína|renminbi|1|CNY|3,783\n" +
                "Dánsko|koruna|1|DKK|3,627\n";

            _testee.Parse(input);

        }


        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void ParseInvalidInputWithoutRate()
        {
            var input =
                "16.09.2015 #180\n" +
                "země|měna|množství|kód|kurz\n" +
                "Austrálie|dolar|1|AUD|\n" +
                "Brazílie|real|1|BRL|6,270\n" +
                "Bulharsko|lev|1|BGN|13,835\n" +
                "Čína|renminbi|1|CNY|3,783\n" +
                "Dánsko|koruna|1|DKK|3,627\n";

            _testee.Parse(input);

        }


        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void ParseInvalidInputWithMissingColumn()
        {
            var input =
                "16.09.2015 #180\n" +
                "země|měna|množství|kód|kurz\n" +
                "Austrálie|1|AUD\n" +
                "Brazílie|real|1|6,270\n" +
                "Bulharsko|lev|BGN|13,835\n" +
                "Čína|1|CNY|3,783\n" +
                "koruna|1|DKK|3,627\n";
            _testee.Parse(input);

        }


    }
}
