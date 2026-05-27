namespace Domain.Entities;

public class Product
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? PictureUrl { get; set; }
    public long Price { get; set; } = 0;
    public int QuantityInStock { get; set; } = 0;
    public DateTime CreatedOnUtc { get; set; }
    public DateTime? PublishedOnUtc { get; set; }
}
