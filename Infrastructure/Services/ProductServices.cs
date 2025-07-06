using Application.Interfaces;
using Domain.Entities;
using SixLabors.ImageSharp;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using ZXing;

namespace Infrastructure.Services
{
    public class ProductServices : IProductServices
    {
        private readonly IGenericRepository<Product> _productRepo;

        public ProductServices(IGenericRepository<Product> productRepo)
        {
            _productRepo = productRepo;
        }
        public async Task AddAsync(Product product, string webPathRoot)
        {
            var id = await _productRepo.AddAsyncThenGetLastId(product);
            product.BarcodeImagePath = GenerateBarcode(id.ToString(), webPathRoot);
            product.Barcode = GenerateRandomBarcode();
            _productRepo.Update(product);
            await _productRepo.SaveChangesAsync();
        }
        public string GenerateBarcode(string content, string webPathRoot, string outputDir = "barcodes")
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
            var filePath = Path.Combine(webPathRoot + "\\" + outputDir, $"{content}.png");

            using var bitmap = new Bitmap(pixelData.Width, pixelData.Height, PixelFormat.Format32bppRgb);
            var bitmapData = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
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
