using Elastic.Clients.Elasticsearch;
using ElasticSearch.API.DTOs;
using ElasticSearch.API.Models;
using ElasticSearch.API.Repositories;
using System.Collections.Immutable;
using System.Net;

namespace ElasticSearch.API.Services
{
    public class ProductService
    {
        private readonly ProductRepository _productRepository;
        private readonly ILogger<Product> _logger;

     

        public ProductService(ProductRepository productRepository , ILogger<Product> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        public async Task<ResponseDto<ProductDto>> SaveAsync(ProductCreateDto request)
        {

            var responseProduct = await _productRepository.SaveAsync(request.CreateProduct());

            if (responseProduct == null)
            {
                return ResponseDto<ProductDto>.Fail(new List<string> { "Kayıt esnasında hata meydana geldi." }, System.Net.HttpStatusCode.InternalServerError);
            }

            return ResponseDto<ProductDto>.Success(responseProduct.CreateDto(), System.Net.HttpStatusCode.Created);
        }

        public async Task<ResponseDto<List<ProductDto>>> GetAllAsync()
        {
            var products = await _productRepository.GetAllAsync();

            var productListDto = products.Select(x => new ProductDto(x.Id, x.Name, x.Price, x.Stock, new ProductFeatureDto(x.Feature.Width, x.Feature.Height, x.Feature!.Color.ToString()))).ToList();


            return ResponseDto<List<ProductDto>>.Success(productListDto, System.Net.HttpStatusCode.OK);
        }

        public async Task<ResponseDto<ProductDto>> GetByIdAsync(string id)
        {

            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
                return ResponseDto<ProductDto>.Fail("ürün bulunamadı", System.Net.HttpStatusCode.NotFound);

            return ResponseDto<ProductDto>.Success(product.CreateDto(), HttpStatusCode.OK);



        }

        public async Task<ResponseDto<bool>> UpdateAsync(ProductUpdateDto updatedProduct)
        {
            var isSuccess = await _productRepository.UpdateAsync(updatedProduct);

            if (!isSuccess)
                return ResponseDto<bool>.Fail("güncelleme yapılamadı", System.Net.HttpStatusCode.NoContent);


            return ResponseDto<bool>.Success(isSuccess, HttpStatusCode.OK);
        }

        public async Task<ResponseDto<bool>> DeleteAsync(string id)
        {
            var deleteResponse = await _productRepository.DeleteAsync(id);

            if (!deleteResponse.IsSuccess() && deleteResponse.Result == Result.NotFound)
            {
                return ResponseDto<bool>.Fail("silmeye çalıştığınız ürün bulunamamıştır", System.Net.HttpStatusCode.NotFound);
            }

            if (!deleteResponse.IsSuccess())
            {
                deleteResponse.TryGetOriginalException(out Exception? exception);
                _logger.LogError(exception, deleteResponse.ElasticsearchServerError.Error.ToString());
                return ResponseDto<bool>.Fail("silme işlemi yapılamadı", System.Net.HttpStatusCode.InternalServerError);
            }

            return ResponseDto<bool>.Success(true, HttpStatusCode.OK);
        }
    }
}
