using ElasticSearch.API.DTOs;
using ElasticSearch.API.Models;
using Nest;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace ElasticSearch.API.Repositories
{
    public class ProductRepository
    {
        private readonly ElasticClient _client;
        private const string indexName = "products";

        public ProductRepository(ElasticClient client)
        {
            _client = client;
        }

        public async Task<Product> SaveAsync(Product newProduct)
        {
            newProduct.Created = DateTime.Now;

            var response = await _client.IndexAsync(newProduct, x => x.Index(indexName).Id(Guid.NewGuid())); //elasticserach de indexleme kayıt  //normalde id kendi atıyor ama biz kendimiz atmak istersek bu şekilde atıyoruz

            if (!response.IsValid) return null;  //fast fail   deniyor bu kullanıma 

            newProduct.Id = response.Id;
            return newProduct;

        }
        public async Task<ImmutableList<Product>> GetAllAsync()
        {
            //Immutablelist -> üzerinde değişiklik yapılamayan liste
            var result = await _client.SearchAsync<Product>(s => s.Index(indexName).Query(q => q.MatchAll()));
            foreach (var hit in result.Hits)
                hit.Source.Id = hit.Id;

            return result.Documents.ToImmutableList();


        }

        public async Task<Product> GetByIdAsync(string id)
        {
            var response = await _client.GetAsync<Product>(id, x => x.Index(indexName));

            if (!response.IsValid)
                return null;

            response.Source.Id= response.Id;
            return response.Source;


        }

        public async Task<bool> UpdateAsync(ProductUpdateDto updatedProduct)
        {
            var response = await _client.UpdateAsync<Product,ProductUpdateDto>(updatedProduct.Id, x => x.Index(indexName).Doc(updatedProduct));

           
            return response.IsValid;


        }

        public async Task<DeleteResponse> DeleteAsync(string id)
        {
            var response = await _client.DeleteAsync<Product>(id, x => x.Index(indexName));


            return response;


        }
    }
}
