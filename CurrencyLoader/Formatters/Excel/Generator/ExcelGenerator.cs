using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using CurrencyLoader.Formatters.Excel.StyleSheet;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace CurrencyLoader.Formatters.Excel.Generator
{
    internal partial class ExcelGenerator
    {
        internal static void BuildWorkbook<T>(ExcelType excelType, Stream filename, IEnumerable<T> data, string title = "")
        {
            try
            {
                using (SpreadsheetDocument xl = SpreadsheetDocument.Create(filename, SpreadsheetDocumentType.Workbook))
                {
                    var wbp = xl.AddWorkbookPart();
                    var wsp = wbp.AddNewPart<WorksheetPart>();
                    var wb = new Workbook();
                    var fv = new FileVersion();
                    fv.ApplicationName = "Microsoft Office Excel";
                    var ws = new Worksheet();
                    var sheetData = new SheetData();
                    var  propertyInfo = typeof(T).GetProperties();


                    var wbsp = wbp.AddNewPart<WorkbookStylesPart>();
                    wbsp.Stylesheet = CreateStylesheet(excelType);
                    wbsp.Stylesheet.Save();



                    Columns columns = new Columns();
                    columns.Append(SetColumnSizes(propertyInfo));                    
                    ws.Append(columns);


                    UInt32 index; //index where excel starts
                    var drawing = GetLogo(excelType, wsp, out index);
                    
                    if (!string.IsNullOrEmpty(title)) sheetData.Append(CreateHeader(title, index++));
                    sheetData.Append(CreateColumnHeader(index++, propertyInfo));


                    foreach (T item in data)
                    {
                        sheetData.Append(CreateContent(index++, item, propertyInfo));
                    }


                    ws.Append(sheetData);
                    if (drawing != null) ws.Append(drawing);
                    wsp.Worksheet = ws;
                    wsp.Worksheet.Save();
                    var sheets = new Sheets();
                    var sheet = new Sheet();
                    sheet.Name = "Sheet1";
                    sheet.SheetId = 1;
                    sheet.Id = wbp.GetIdOfPart(wsp);
                    sheets.Append(sheet);
                    wb.Append(fv);
                    wb.Append(sheets);

                    xl.WorkbookPart.Workbook = wb;
                    xl.WorkbookPart.Workbook.Save();
                    xl.Close();
                }
            }
            catch 
            {
                throw;
            }
        }


        private static IEnumerable<Column> SetColumnSizes(IEnumerable<PropertyInfo> properties)
        {
            uint index = 1;
            foreach (var property in properties)
            {                                
                var customAttributes = property.GetCustomAttributes(typeof(ColumnAttribute), false);
                if (customAttributes == null || customAttributes.Length != 1) continue;
                yield return CreateColumnSize(index , index++, ((ColumnAttribute)customAttributes[0]).ColumnWidth);                
            }
        }



        private static Column CreateColumnSize(UInt32 startColumnIndex, UInt32 endColumnIndex, double columnWidth)
        {
            Column column;
            column = new Column();
            column.Min = startColumnIndex;
            column.Max = endColumnIndex;
            column.Width = columnWidth;
            column.CustomWidth = true;
            return column;
        }


        internal static Stylesheet CreateStylesheet(ExcelType excelType)
        {
            return StyleSheet.StyleSheet.GetStyleSheet(excelType);
        }

        internal static Row CreateHeader(string title, UInt32 index)
        {
            var r = new Row();
            r.RowIndex = index;

            var c = new Cell();
            c.DataType = CellValues.String;
            c.StyleIndex = (uint) CellTypeCode.Title;
            c.CellReference = "A" + index;
            c.CellValue = new CellValue(title);
            r.Append(c);

            return r;
        }

        internal static Row CreateColumnHeader(UInt32 index, IEnumerable<PropertyInfo> properties)
        {
            var r = new Row();
            r.RowIndex = index;
            int cellIndex = 0;
            foreach (PropertyInfo property in properties)
            {
                var columnAttribute = property.GetCustomAttributes(typeof (ColumnAttribute), false);
                if (columnAttribute == null || columnAttribute.Length != 1) continue;
                var cell = new Cell();
                cell.DataType = CellValues.String;
                cell.StyleIndex = (uint) CellTypeCode.Header;
                cell.CellReference = ('A' + cellIndex).ToString() + index;
                cell.CellValue = new CellValue(((ColumnAttribute) columnAttribute[0]).Description);
                r.Append(cell);
                cellIndex++;
            }
            return r;
        }

        internal static Row CreateContent(UInt32 index, object item, IEnumerable<PropertyInfo> properties)
        {
            var r = new Row();
            r.RowIndex = index;
            var cellIndex = 0;
            foreach (PropertyInfo property in properties)
            {
                var attr = property.GetCustomAttributes(typeof (ColumnAttribute), false);
                if (attr == null || attr.Length != 1) continue;
                var cell = new Cell();
                cell.CellReference = ('A' + cellIndex).ToString() + index;
                cell.StyleIndex = (uint)CellTypeCode.Content;
                cell.CellValue = property.PropertyType == typeof (DateTime)
                                     ? new CellValue(
                                           ((DateTime) property.GetValue(item, null)).ToString("dd.MM.yyyy"))
                                     : new CellValue(string.Format("{0}", property.GetValue(item, null)));
                if (property.PropertyType == typeof(int) ||
                    property.PropertyType == typeof(short) ||
                    property.PropertyType == typeof(long))
                {
                    cell.DataType = CellValues.Number;
                    cell.StyleIndex = (uint)CellTypeCode.Int;
                }else
                if (property.PropertyType == typeof(decimal) ||                    
                    property.PropertyType == typeof(double) ||
                    property.PropertyType == typeof(float))
                {
                    cell.DataType = CellValues.Number;
                    decimal d;
                    if (decimal.TryParse(string.Format("{0}", property.GetValue(item, null)), out d))
                    {
                        var x = decimal.Truncate(d);
                        if (d - x > 0)
                            cell.StyleIndex = (uint)CellTypeCode.Decimal4;
                    }                                        
                }
                else
                    cell.DataType = CellValues.String;                
                r.Append(cell);
            }
            return r;
        }
    }
}