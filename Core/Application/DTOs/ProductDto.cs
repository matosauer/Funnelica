namespace Application.DTOs
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public string? PictureUrl { get; set; }
        public long Price { get; set; }
        public int QuantityInStock { get; set; }
    }
}