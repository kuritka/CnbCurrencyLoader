using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;

namespace CurrencyLoader.Formatters.Excel.StyleSheet
{
    internal partial class StyleSheet
    {
        #region CellFormats enum

        enum CellFormats
        {
            Empty = 0,
            DateTimeFormat = 164,
            Decimal4Format = 165,
            Decimal2Format = 166,
            TextFormat = 167,
            DateFormat = 168,
            IntFormat = 169,
        }

        #endregion

        #region Nested type: CellContentFormat

        private class CellContentFormat : NumberingFormats
        {
            public CellContentFormat()
            {
                Append(CreateFormat("dd/mm/yyyy hh:mm:ss", CellFormats.DateTimeFormat));
                Append(CreateFormat("#,##0.0000", CellFormats.Decimal4Format));
                Append(CreateFormat("#,##0.00", CellFormats.Decimal2Format));
                Append(CreateFormat("@", CellFormats.TextFormat));
                Append(CreateFormat("dd.MM.yyyy", CellFormats.DateFormat));
                Append(CreateFormat("#,#0.0", CellFormats.IntFormat));
                Count = UInt32Value.FromUInt32((uint)base.ChildElements.Count);
            }


            //public CellContentFormat()
            //{
            //    Append(CreateFormat("dd/mm/yyyy hh:mm:ss", CellFormats.DateTimeFormat));
            //    Append(CreateFormat("0.0000", CellFormats.Decimal4Format));
            //    Append(CreateFormat("0.00", CellFormats.Decimal2Format));
            //    Append(CreateFormat("@", CellFormats.TextFormat));
            //    Append(CreateFormat("dd.MM.yyyy", CellFormats.DateFormat));
            //    Append(CreateFormat("0.0", CellFormats.IntFormat));
            //    Count = UInt32Value.FromUInt32((uint)base.ChildElements.Count);
            //}



            private static NumberingFormat CreateFormat(string format, CellFormats formatIndex)
            {
                var numberingFormat = new NumberingFormat();
                numberingFormat.NumberFormatId = UInt32Value.FromUInt32((uint) formatIndex);
                numberingFormat.FormatCode = StringValue.FromString(format);
                return numberingFormat;
            }
        }

        #endregion
    }
}