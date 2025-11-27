using WebClient.Validation;

namespace WebClient.DTOs
{
    public class ReadProductDTO
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string CustomerName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? OrderAddress { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime DeliveryDate { get; set; }
    }

    public class CreateProductDTO
    {
        public string CustomerName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? OrderAddress { get; set; }
        public DateTime DeliveryDate { get; set; }
    }

    public class UpdateProductDTO
    {
        public string CustomerName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? OrderAddress { get; set; }
        public DateTime DeliveryDate { get; set; }
    }
}
