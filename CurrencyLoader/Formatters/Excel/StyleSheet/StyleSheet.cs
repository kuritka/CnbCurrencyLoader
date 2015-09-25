using System;
using System.Collections.Generic;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;

namespace CurrencyLoader.Formatters.Excel.StyleSheet
{
    internal partial class StyleSheet
    {


        private static IEnumerable<OpenXmlElement> GetStyleSheetScheme(ExcelType scheme)
        {
            switch (scheme)
            {
                case ExcelType.Coba:
                case ExcelType.CobaWithLogo: 
                    return CellStyles.CreateCeexStyle();
                case ExcelType.Empty: 
                    return CellStyles.CreateEmptyStyle();
            }
            throw new NotImplementedException("There is not implementation for selected color scheme");
        }

        /// <summary>
        /// Creates Stylesheet from collections of border styles, fontstyles, color styles etc...
        /// </summary>        
        public static  Stylesheet  GetStyleSheet(ExcelType scheme)
        {
            var stylesheet = new Stylesheet();
            var fontCollection = new FontCollection();
            var fillsCollection = new FillsCollection();
            var borderCollection = new BorderCollection();
            var cellStyleFormats = new CellStyleFormats();

            var cellStyle = new CellFormat();
            cellStyle.NumberFormatId = 0;
            cellStyle.FontId = 0;
            cellStyle.FillId = 0;
            cellStyle.BorderId = 0;
            cellStyleFormats.Append(cellStyle);
            cellStyleFormats.Count = UInt32Value.FromUInt32((uint)cellStyleFormats.ChildElements.Count);

            var numberingFormats = new CellContentFormat(); 

            var cellFormats = new DocumentFormat.OpenXml.Spreadsheet.CellFormats();
            cellFormats.Append(GetStyleSheetScheme(scheme));
            cellFormats.Count = UInt32Value.FromUInt32((uint)cellFormats.ChildElements.Count);

            stylesheet.Append(numberingFormats);
            stylesheet.Append(fontCollection);
            stylesheet.Append(fillsCollection);
            stylesheet.Append(borderCollection);
            stylesheet.Append(cellStyleFormats);
            stylesheet.Append(cellFormats);

            var css = new DocumentFormat.OpenXml.Spreadsheet.CellStyles();
            var cs = new CellStyle();
            cs.Name = StringValue.FromString("Normal");
            cs.FormatId = 0;
            cs.BuiltinId = 0;
            css.Append(cs);
            css.Count = UInt32Value.FromUInt32((uint)css.ChildElements.Count);
            stylesheet.Append(css);

            var dfs = new DifferentialFormats();
            dfs.Count = 0;
            stylesheet.Append(dfs);

            var tss = new TableStyles();
            tss.Count = 0;
            tss.DefaultTableStyle = StringValue.FromString("TableStyleMedium9");
            tss.DefaultPivotStyle = StringValue.FromString("PivotStyleLight16");
            stylesheet.Append(tss);

            return stylesheet;
        }

      
    }
}
