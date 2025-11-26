using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    public class Products
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [MaxLength(100)]
        [Required]
        public string CustomerName { get; set; } = string.Empty;
        [MaxLength(100)]
        [Required]
        public string PhoneNumber { get; set; } = string.Empty;
        [MaxLength(500)]
        [Required]
        public string Description { get; set; } = string.Empty;
        [MaxLength(500)]
        public string? OrderAddress { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [Required]
        public DateTime DeliveryDate { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime DeletedAt { get; set; }
    }
}
