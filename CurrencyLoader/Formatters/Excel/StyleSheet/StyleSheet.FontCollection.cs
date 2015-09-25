using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;

namespace CurrencyLoader.Formatters.Excel.StyleSheet
{
    partial class StyleSheet
    {
        private class FontCollection : Fonts
        {

            public FontCollection()
            {
                Append(CreateFont());
                Append(CreateFont("Palatino Linotype", 18));
                Append(CreateFont("Nina"));
                Append(CreateFont("Nina", 16));
                Append(CreateFont("Calibri", 11, true, "FEFEFE"));
                Count = UInt32Value.FromUInt32((uint)base.ChildElements.Count);
            }


            private static Font CreateFont(string fontName = "Calibri", int fontSize = 11, bool bold = false, string color = "00000000")
            {
                var ft = new Font();
                var ftn = new FontName();
                ftn.Val = StringValue.FromString(fontName);
                var ftsz = new FontSize();
                ftsz.Val = DoubleValue.FromDouble(fontSize);
                ft.FontName = ftn;
                ft.FontSize = ftsz;
                ft.Bold = new Bold { Val = bold };
                ft.Color = new Color { Rgb = HexBinaryValue.FromString(color) };
                return ft;
            }
            
        }

        enum FontStyles
        {
            StandardCalibri = 0,
            BigPalatino = 1,
            StandardNina = 2,
            BigNina = 3,
            StandardCalibriBold = 4,
        }
    }
}
