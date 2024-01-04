using ElasticSearch.API.Models;

namespace ElasticSearch.API.DTOs
{
    public record ProductCreateDto(string Name, decimal Price, int Stock, ProductFeatureDto Feature)  //propla tanımlamak yerine record class da böyle de tanımlanabiliyor.
    {
        public Product CreateProduct()  //service de modelleme yapıcağıma burda yaptım
        {
            return new Product()
            {
                Name = Name,
                Price = Price,
                Stock = Stock,
                Feature = new ProductFeature() { Width = Feature.Width, Height = Feature.Height, Color =(EColor)int.Parse( Feature.Color)}
            };
        }
    }
}
