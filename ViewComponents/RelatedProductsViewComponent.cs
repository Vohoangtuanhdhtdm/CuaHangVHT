using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using CuaHangVHT.Models; // Thay bằng namespace của Product
using CuaHangVHT.Services;

namespace CuaHangVHT.ViewComponents
{
    public class RelatedProductsViewComponent : ViewComponent
    {
        private readonly IProductService _productService;

        public RelatedProductsViewComponent(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int productId)
        {
            var relatedProducts = await _productService.GetRelatedProductsAsync(productId);
            return View(relatedProducts); // Trả dữ liệu cho View
        }
    }
}
