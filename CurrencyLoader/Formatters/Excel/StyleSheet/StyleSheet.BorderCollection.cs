using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;

namespace CurrencyLoader.Formatters.Excel.StyleSheet
{
    partial class StyleSheet
    {
        private  class BorderCollection : Borders
        {
            public BorderCollection()
            {
                Append(CreateNoBorder());
                Append(CreateThinBorder());
                Count = UInt32Value.FromUInt32((uint)base.ChildElements.Count);
            }


            private static Border CreateNoBorder()
            {
                var border = new Border();
                border.LeftBorder = new LeftBorder();
                border.RightBorder = new RightBorder();
                border.TopBorder = new TopBorder();
                border.BottomBorder = new BottomBorder();
                border.DiagonalBorder = new DiagonalBorder();
                return border;
            }

            private static Border CreateThinBorder()
            {
                var border = new Border();
                border.LeftBorder = new LeftBorder();
                border.LeftBorder.Style = BorderStyleValues.Thin;
                border.RightBorder = new RightBorder();
                border.RightBorder.Style = BorderStyleValues.Thin;
                border.TopBorder = new TopBorder();
                border.TopBorder.Style = BorderStyleValues.Thin;
                border.BottomBorder = new BottomBorder();
                border.BottomBorder.Style = BorderStyleValues.Thin;
                border.DiagonalBorder = new DiagonalBorder();
                return border;
            }

          
        }

        enum BorderStyles
        {
            NoBorder = 0,
            ThinBorder = 1
        }
    }
}
