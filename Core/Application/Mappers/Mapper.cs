using Application.DTOs;
using Domain.Entities;

namespace Application.Mappers
{
    public static class Mapper
    {
        //Product specific mapping
        public static TDestination Map<TDestination>(Product source)
        {
            switch (typeof(TDestination))
            {
                case Type t when t == typeof(ProductDto):
                    return (TDestination)(object)new ProductDto
                    {
                        Id = source.Id,
                        Name = source.Name,
                        Price = source.Price,
                        Description = source.Description,
                        PictureUrl = source.PictureUrl,
                        QuantityInStock = source.QuantityInStock
                    };
                default:
                    throw new NotSupportedException($"Mapping for {typeof(TDestination)} is not supported.");
            }
        }

        public static TDestination Map<TDestination>(IEnumerable<Product> source)
        {
            switch (typeof(TDestination))
            {
                case Type t when t == typeof(IEnumerable<ProductDto>):
                    var destinationList = new List<ProductDto>();
                    foreach (var item in source)
                    {
                        destinationList.Add(Map<ProductDto>(item));
                    }
                    return (TDestination)(object)destinationList;
                default:
                    throw new NotSupportedException($"Mapping for {typeof(TDestination)} is not supported.");
            }
        }


        //ProductDto to entity mapping

        public static TDestination Map<TDestination>(ProductDto source)
        {
            switch (typeof(TDestination))
            {
                case Type t when t == typeof(Product):
                    return (TDestination)(object)new Product
                    {
                        Id = source.Id,
                        Name = source.Name,
                        Price = source.Price,
                        Description = source.Description,
                        PictureUrl = source.PictureUrl,
                        QuantityInStock = source.QuantityInStock
                    };
                default:
                    throw new NotSupportedException($"Mapping for {typeof(TDestination)} is not supported.");
            }
        }

        public static TDestination Map<TDestination>(IEnumerable<ProductDto> source)
        {
            switch (typeof(TDestination))
            {
                case Type t when t == typeof(IEnumerable<Product>):
                    var destinationList = new List<Product>();
                    foreach (var item in source)
                    {
                        destinationList.Add(Map<Product>(item));
                    }
                    return (TDestination)(object)destinationList;
                default:
                    throw new NotSupportedException($"Mapping for {typeof(TDestination)} is not supported.");
            }
        }

    }
}
