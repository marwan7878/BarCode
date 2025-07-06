using Domain.Entities;

namespace Application.Interfaces
{
    public interface IProductServices
    {
        Task AddAsync(Product product, string webPathRoot);
        string GenerateBarcode(string content, string webPathRoot, string outputDir);
        Task<Product?> GetByIdAsync(int id);
        Task<IEnumerable<Product>> GetAllAsync();
        Task UpdateAsync(Product product);
        Task DeleteAsync(int id);
        string GenerateRandomBarcode();
    }
}
