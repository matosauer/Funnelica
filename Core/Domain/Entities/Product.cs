namespace Domain.Entities;

public class Product
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string? Description { get; set; }
    public required string? PictureUrl { get; set; }
    public long Price { get; set; }
    public int QuantityInStock { get; set; }
    public DateTime CreatedOnUtc { get; set; }
    public DateTime? PublishedOnUtc { get; set; }
}
