using ElasticSearch.API.Models;

namespace ElasticSearch.API.DTOs
{
    public record ProductFeatureDto(int Width,int Height , string Color) // record da prop yerine böyle de tanımlanabilir.
    {
    }
}
