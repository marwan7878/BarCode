using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace BarCode.Services
{
    public class ProductServices
    {
        public static string GenerateBarcode(string content, string outputDir)
        {
            var writer = new ZXing.BarcodeWriterPixelData
            {
                Format = ZXing.BarcodeFormat.CODE_128,
                Options = new ZXing.Common.EncodingOptions
                {
                    Height = 80,
                    Width = 300,
                    Margin = 2
                }
            };

            var pixelData = writer.Write(content);
            var filePath = Path.Combine(outputDir, $"{content}.png");

            using var bitmap = new Bitmap(pixelData.Width, pixelData.Height, PixelFormat.Format32bppRgb);
            var bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                                             ImageLockMode.WriteOnly, PixelFormat.Format32bppRgb);
            Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0, pixelData.Pixels.Length);
            bitmap.UnlockBits(bitmapData);
            bitmap.Save(filePath, ImageFormat.Png);

            return filePath;
        }
        //public static string DecodeBarcode(IFormFile file)
        //{
        //    using var stream = file.OpenReadStream();
        //    using var bitmap = new Bitmap(stream);
        //    var reader = new ZXing.BarcodeReader();
        //    var result = reader.Decode(bitmap);
        //    return result?.Text;
        //}

    }
}
