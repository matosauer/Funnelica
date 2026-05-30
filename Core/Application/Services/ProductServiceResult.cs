using Application.DTOs;

namespace Application.Services
{
    public enum FailureType
    {
        NotFound, Validation, BusinessRule, Conflict, Unknown
    }

    public class ProductServiceResult
    {
        public bool Success { get; set; }

        public FailureType? FailureType { get; set; }

        public string? Message { get; set; } = null;

        public List<string> ValidationErrors { get; set; } = new List<string>();

        public ProductDto? ProductDto { get; set; } = null;
    }
}
