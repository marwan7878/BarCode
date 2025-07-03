using BarCode.Models;

namespace BarCode.Services.Interfaces
{
    public interface IProductServices
    {
        Task AddAsync(Product product);
        string GenerateBarcode(string content, string outputDir);
        Task<Product?> GetByIdAsync(int id);
        string DecodeBarcode(IFormFile file);
        Task<IEnumerable<Product>> GetAllAsync();
        Task UpdateAsync(Product product);
        Task DeleteAsync(int id);
        string GenerateRandomBarcode();
    }
}
