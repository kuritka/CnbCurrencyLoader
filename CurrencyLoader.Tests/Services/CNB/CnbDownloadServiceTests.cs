using System;
using CurrencyLoader.Services.Cnb;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CurrencyLoader.Tests.Services.CNB
{
    [TestClass]
    public class CnbDownloadServiceTests
    {

        private readonly CnbDownloadService _testee;
        public CnbDownloadServiceTests()
        {
            _testee = new CnbDownloadService();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DownloadForMinimalDate()
        {
             _testee.Download(DateTime.MinValue);
        }



        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DownloadForTomorow()
        {
           _testee.Download(DateTime.Now.AddDays(1));
           
        }


        [TestMethod]
        public void DownloadForToday()
        {
            var result = _testee.Download(DateTime.Now);
            result.Contains(""+DateTime.Today);
        }

        [TestMethod]
        public void DownloadForTwoWeeksAgo()
        {
            var result =_testee.Download(DateTime.Now.AddDays(-14));
            result.Contains("" + DateTime.Today.AddDays(-14));
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DownloadForOneYearAgo()
        {
           _testee.Download(DateTime.Today.AddYears(-1));
 
        }


        [TestMethod]
        
        public void DownloadForOneYearMinusOneDayAgo()
        {
            var result =_testee.Download(DateTime.Today.AddYears(-1).AddDays(1));
            result.Contains("" + DateTime.Today.AddYears(-1).AddDays(1));
            }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DownloadForOneYearAndOneDayAgo()
        {
            _testee.Download(DateTime.Now.AddYears(-1).AddDays(-1));
        }



    }
}
