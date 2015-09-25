using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;

namespace CurrencyLoader.Formatters.Excel.StyleSheet
{
    internal partial class StyleSheet
    {
        #region FillStyles enum

        enum FillStyles
        {
            Empty = 0,
            Gray = 1,
            Orange = 2,
            CeexRed = 3
        }

        #endregion

        #region Nested type: FillsCollection

        private class FillsCollection : Fills
        {
            public FillsCollection()
            {
                Append(CreateEmptyFill());
                Append(CreateGrayFill());
                Append(CreateOrangeFill());
                Append(CreateCeexRedFill());
                Count = UInt32Value.FromUInt32((uint) base.ChildElements.Count);
            }


            private static Fill CreateEmptyFill()
            {
                var fill = new Fill();
                var patternFill = new PatternFill();
                patternFill.PatternType = PatternValues.None;
                return fill;
            }


            private static Fill CreateGrayFill()
            {
                var fill = new Fill();
                var patternFill = new PatternFill();
                patternFill.PatternType = PatternValues.Gray125;
                fill.PatternFill = patternFill;
                return fill;
            }


            private static Fill CreateOrangeFill()
            {
                Fill fill = CreateColoredFill();
                fill.PatternFill.ForegroundColor.Rgb = HexBinaryValue.FromString("00ff9728");
                return fill;
            }


            private static Fill CreateCeexRedFill()
            {
                Fill fill = CreateColoredFill();
                fill.PatternFill.ForegroundColor.Rgb = HexBinaryValue.FromString("00D52121");
                //fill.PatternFill.ForegroundColor.Rgb = HexBinaryValue.FromString("00FEFEFE");
                return fill;
            }


            private static Fill CreateColoredFill()
            {
                var fill = new Fill();
                var patternFill = new PatternFill();
                patternFill.PatternType = PatternValues.Solid;
                patternFill.ForegroundColor = new ForegroundColor();
                patternFill.BackgroundColor = new BackgroundColor();
                fill.PatternFill = patternFill;
                return fill;
            }
        }

        #endregion
    }
}