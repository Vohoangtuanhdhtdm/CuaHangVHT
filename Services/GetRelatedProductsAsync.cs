using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CuaHangVHT.Data;
using CuaHangVHT.Models; // Thay bằng namespace của model Product

namespace CuaHangVHT.Services
{
    public interface IProductService
    {
        /// <summary>
        /// Lấy danh sách sản phẩm có danh mục khác với sản phẩm hiện tại.
        /// </summary>
        /// <param name="productId">ID của sản phẩm hiện tại.</param>
        /// <returns>Danh sách sản phẩm liên quan.</returns>
        Task<List<Product>> GetRelatedProductsAsync(int productId);
    }

    public class ProductService : IProductService
    {
        private readonly TuanStoreContext _db;

        public ProductService(TuanStoreContext context)
        {
            _db = context;
        }

        public async Task<List<Product>> GetRelatedProductsAsync(int productId)
        {
            // Lấy sản phẩm hiện tại
            var currentProduct = await _db.Products.FindAsync(productId);
            if (currentProduct == null)
            {
                return new List<Product>(); // Không tìm thấy sản phẩm
            }

            // Lấy danh mục của sản phẩm hiện tại
            var currentCategoryId = currentProduct.CategoryId;

            // Lọc các sản phẩm thuộc danh mục khác
            var relatedProducts = _db.Products
                .Where(pr => pr.CategoryId != currentCategoryId && pr.ProductId != productId)
                .ToList();

            return relatedProducts;
        }
    }
}
