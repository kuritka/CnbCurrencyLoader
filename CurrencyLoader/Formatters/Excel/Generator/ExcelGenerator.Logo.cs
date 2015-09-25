using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Drawing.Spreadsheet;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using BlipFill = DocumentFormat.OpenXml.Drawing.Spreadsheet.BlipFill;
using NonVisualDrawingProperties = DocumentFormat.OpenXml.Drawing.Spreadsheet.NonVisualDrawingProperties;
using NonVisualPictureDrawingProperties = DocumentFormat.OpenXml.Drawing.Spreadsheet.NonVisualPictureDrawingProperties;
using NonVisualPictureProperties = DocumentFormat.OpenXml.Drawing.Spreadsheet.NonVisualPictureProperties;
using Picture = DocumentFormat.OpenXml.Drawing.Spreadsheet.Picture;
using Position = DocumentFormat.OpenXml.Drawing.Spreadsheet.Position;
using ShapeProperties = DocumentFormat.OpenXml.Drawing.Spreadsheet.ShapeProperties;

namespace CurrencyLoader.Formatters.Excel.Generator
{
    partial class ExcelGenerator
    {

        private static string GetLogoPath(ExcelType excelType)
        {
            switch (excelType)
            {                
                case ExcelType.CobaWithLogo: return @"_img\CEEX03_small.png";
                case ExcelType.Coba: return string.Empty;
                default: return string.Empty;
            }
        }



        private static Drawing GetLogo(ExcelType excelType, WorksheetPart wsp, out uint startIndex)
        {

            var dp = wsp.AddNewPart<DrawingsPart>();
            Drawing drawing;
            var sImagePath = GetLogoPath(excelType);
            if (string.IsNullOrEmpty(sImagePath))
            {
                startIndex = 1;
                return null;
            }
            startIndex = 8;
            //sorry, I must do it in this way, MEmory stream oesn't work in FeedData. There are several discussions in the internet about it.
            ImagePart imgp = dp.AddImagePart(ImagePartType.Png, wsp.GetIdOfPart(dp));
            var bm = new Bitmap(Resource1.logo);
            //var bm = new Bitmap(@"ExcelProvider\_img\CEEX_small.png");
            using (var ms = new MemoryStream())
            {
                bm.Save(ms,ImageFormat.Png);
                ms.Seek(0, 0);
                imgp.FeedData(ms);
            }
         
            var nvdp = new NonVisualDrawingProperties();
            nvdp.Id = 1025;
            nvdp.Name = "logo";
            nvdp.Description = "polymathlogo";
            var picLocks = new PictureLocks();
            picLocks.NoChangeAspect = true;
            picLocks.NoChangeArrowheads = true;
            var nvpdp = new NonVisualPictureDrawingProperties();
            nvpdp.PictureLocks = picLocks;
            var nvpp = new NonVisualPictureProperties();
            nvpp.NonVisualDrawingProperties = nvdp;
            nvpp.NonVisualPictureDrawingProperties = nvpdp;

            var stretch = new Stretch();
            stretch.FillRectangle = new FillRectangle();

            var blipFill = new BlipFill();
            var blip = new Blip();
            blip.Embed = dp.GetIdOfPart(imgp);
            blip.CompressionState = BlipCompressionValues.Print;
            blipFill.Blip = blip;
            blipFill.SourceRectangle = new SourceRectangle();
            blipFill.Append(stretch);

            var t2d = new Transform2D();
            var offset = new Offset();
            offset.X = 0;
            offset.Y = 0;
            t2d.Offset = offset;          
            //http://en.wikipedia.org/wiki/English_Metric_Unit#DrawingML
            //http://stackoverflow.com/questions/1341930/pixel-to-centimeter
            //http://stackoverflow.com/questions/139655/how-to-convert-pixels-to-points-px-to-pt-in-net-c
            var extents = new Extents();
            extents.Cx = bm.Width * (long)(914400 / bm.HorizontalResolution);
            extents.Cy = bm.Height * (long)(914400 / bm.VerticalResolution);
            bm.Dispose();
            t2d.Extents = extents;
            var sp = new ShapeProperties();
            sp.BlackWhiteMode = BlackWhiteModeValues.Auto;
            sp.Transform2D = t2d;
            var prstGeom = new PresetGeometry();
            prstGeom.Preset = ShapeTypeValues.Rectangle;
            prstGeom.AdjustValueList = new AdjustValueList();
            sp.Append(prstGeom);
            sp.Append(new NoFill());

            var picture = new Picture();
            picture.NonVisualPictureProperties = nvpp;
            picture.BlipFill = blipFill;
            picture.ShapeProperties = sp;

            var pos = new Position();
            pos.X = 0;
            pos.Y = 0;
            var ext = new Extent();
            ext.Cx = extents.Cx;
            ext.Cy = extents.Cy;
            var anchor = new AbsoluteAnchor();
            anchor.Position = pos;
            anchor.Extent = ext;
            anchor.Append(picture);
            anchor.Append(new ClientData());
            var wsd = new WorksheetDrawing();
            wsd.Append(anchor);
            drawing = new Drawing();
            drawing.Id = dp.GetIdOfPart(imgp);
            wsd.Save(dp);
            return drawing;
        }



     

        
    }
}
