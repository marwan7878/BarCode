using System.ComponentModel.DataAnnotations;

namespace BarCode.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        
        public string? Barcode { get; set; }

        public string? BarcodeImagePath { get; set; }

        public int Quantity { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

}
