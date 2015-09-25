using System.Collections.Generic;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;

namespace CurrencyLoader.Formatters.Excel.StyleSheet
{
    partial class StyleSheet
    {

        /// <summary>
        /// good known values for cell styles
        /// This styles are applied by particular cells!!! 
        /// This is style for cell, not for the column!! 150 columns can use only this eight styles
        /// </summary>
        private static class CellStyles
        {
            public static IEnumerable<CellFormat> CreateEmptyStyle()
            {
                yield return CreateCellStyle(CellFormats.Empty, false);
                yield return CreateCellStyle(CellFormats.TextFormat, false);
                yield return CreateCellStyle(CellFormats.DateTimeFormat, false);
                yield return CreateCellStyle(CellFormats.TextFormat);
                yield return CreateCellStyle(CellFormats.TextFormat);
                yield return CreateCellStyle(CellFormats.TextFormat);
                yield return CreateCellStyle(CellFormats.Decimal2Format);
                yield return CreateCellStyle(CellFormats.TextFormat);
            }


            public static IEnumerable<CellFormat> CreateCeexStyle()
            {
                yield return CreateCellStyle(CellFormats.Empty, false);
                yield return CreateCellStyle(CellFormats.TextFormat, false);
                yield return CreateCellStyle(CellFormats.Decimal4Format, false);
                yield return CreateCellStyle(CellFormats.DateFormat, false);
                yield return CreateCellStyle(CellFormats.TextFormat);
                yield return CreateCellStyle(CellFormats.TextFormat, true, FontStyles.BigNina); // title
                yield return CreateCellStyle(CellFormats.TextFormat, true, FontStyles.StandardCalibriBold, BorderStyles.ThinBorder, FillStyles.CeexRed); //header
                yield return CreateCellStyle(CellFormats.Decimal2Format, true, FontStyles.StandardCalibri, BorderStyles.ThinBorder); //data cell 
                yield return CreateCellStyle(CellFormats.Decimal4Format, true, FontStyles.StandardCalibri, BorderStyles.ThinBorder); //data decimal 4 
                yield return CreateCellStyle(CellFormats.IntFormat, true, FontStyles.StandardCalibri, BorderStyles.ThinBorder); //data cell 
            }



            private static CellFormat CreateCellStyle(CellFormats format, bool applyNumberFormat = true
                , FontStyles fontStyle = FontStyles.StandardCalibri
                , BorderStyles borderStyle = BorderStyles.NoBorder
                , FillStyles fillStyle = FillStyles.Empty)
            {
                var cellFormat = new CellFormat();
                cellFormat.NumberFormatId = (uint)format;
                cellFormat.FontId = (uint)fontStyle;
                cellFormat.FillId = (uint)fillStyle;
                cellFormat.BorderId = (uint)borderStyle;
                cellFormat.FormatId = 0;
                cellFormat.ApplyNumberFormat = BooleanValue.FromBoolean(applyNumberFormat);

                return cellFormat;
            }

        }
        
    }


    enum CellTypeCode
    {
        Title = 5,
        Header = 6,
        Content = 7,
        Decimal4 = 8,
        Int = 9,
    }
}
