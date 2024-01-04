using Elastic.Clients.Elasticsearch;
using Elastic.Transport;


namespace ElasticSearch.API.Extensions
{
    public static class ElasticsearchExt
    {
        public static void AddElastic(this IServiceCollection services , IConfiguration configuration)
        {
            var userName = configuration.GetSection("Elastic")["Username"].ToString();
            var password = configuration.GetSection("Elastic")["Password"].ToString();
            var settings = new ElasticsearchClientSettings(new Uri(configuration.GetSection("Elastic")["Url"]!)).Authentication(new BasicAuthentication(userName,password));
       
            var client = new ElasticsearchClient(settings);

            //nest için
            //var pool = new SingleNodeConnectionPool(new Uri(configuration.GetSection("Elastic")["Url"]!));
            //var settings = new ConnectionSettings(pool);
            ////settings.BasicAuthentication();  //username password u burdan yazabiliriz.
            //var client = new ElasticClient(settings);
            services.AddSingleton(client); // bunu elasticsearch dökümanında böyle kullan diyor.
        }
    }
}
