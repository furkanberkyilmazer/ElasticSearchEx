using Elasticsearch.Net;
using Nest;

namespace ElasticSearch.API.Extensions
{
    public static class ElasticsearchExt
    {
        public static void AddElastic(this IServiceCollection services , IConfiguration configuration)
        {

            var pool = new SingleNodeConnectionPool(new Uri(configuration.GetSection("Elastic")["Url"]!));
            var settings = new ConnectionSettings(pool);
            //settings.BasicAuthentication();  //username password u burdan yazabiliriz.
            var client = new ElasticClient(settings);
            services.AddSingleton(client); // bunu elasticsearch dökümanında böyle kullan diyor.
        }
    }
}
