using BarCode.Models;
using BarCode.Services.Interfaces;
using IronBarCode;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using ZXing;

namespace BarCode.Services
{
    public class ProductServices : IProductServices
    {
        private readonly IWebHostEnvironment _env;
        private readonly IGenericRepository<Product> _productRepo;

        public ProductServices(IWebHostEnvironment env, IGenericRepository<Product> productRepo)
        {
            _env = env;
            _productRepo = productRepo;
        }
        public async Task AddAsync(Product product)
        {
            var id = await _productRepo.AddAsyncThenGetLastId(product);
            product.BarcodeImagePath = GenerateBarcode(id.ToString());
            product.Barcode = GenerateRandomBarcode();
            _productRepo.Update(product);
            await _productRepo.SaveChangesAsync();
        }
        public string GenerateBarcode(string content, string outputDir = "barcodes")
        {
            var writer = new BarcodeWriterPixelData
            {
                Format = BarcodeFormat.CODE_128,
                Options = new ZXing.Common.EncodingOptions
                {
                    Height = 80,
                    Width = 300,
                    Margin = 2
                }
            };

            var pixelData = writer.Write(content);
            var filePath = Path.Combine(_env.WebRootPath + "\\" + outputDir, $"{content}.png");

            using var bitmap = new Bitmap(pixelData.Width, pixelData.Height, PixelFormat.Format32bppRgb);
            var bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                                             ImageLockMode.WriteOnly, PixelFormat.Format32bppRgb);
            Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0, pixelData.Pixels.Length);
            bitmap.UnlockBits(bitmapData);
            bitmap.Save(filePath, ImageFormat.Png);

            return $"/{outputDir}/{content}.png";
        }
        public async Task<Product?> GetByIdAsync(int id)
        {
            var appointment = await _productRepo.GetByIdAsync(a => a.Id == id);
            return appointment;
        }
        public string DecodeBarcode(IFormFile file)
        {
            using var stream = file.OpenReadStream();

            // Read barcode from stream
            var results = BarcodeReader.Read(stream);

            // Return the first decoded value, if any
            return results.Any() ? results.First().Text : "";
        }
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _productRepo.GetAllAsync();
        }
        public async Task UpdateAsync(Product updatedProduct)
        {
            var existingProduct = await _productRepo.GetByIdAsync(updatedProduct.Id);
            if (existingProduct == null)
                throw new InvalidOperationException("Product not found.");

            // Only update selected properties
            existingProduct.Name = updatedProduct.Name;
            existingProduct.Quantity = updatedProduct.Quantity;
            // Do NOT update: Barcode, CreatedAt, BarcodeImagePath

            _productRepo.Update(existingProduct);
            await _productRepo.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _productRepo.GetByIdAsync(id);
            if (product != null)
            {
                _productRepo.Delete(product);
                await _productRepo.SaveChangesAsync();
            }
        }
        public string GenerateRandomBarcode()
        {
            var random = new Random();
            return string.Concat(Enumerable.Range(0, 10).Select(_ => random.Next(0, 10).ToString()));
        }
    }
}
