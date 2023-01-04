using Business.Abstract;
using Entity.Concrete;
using Entity.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    //[Authorize(Roles = "User")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private IProductService _productService;
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            var result = _productService.GetAll();
            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpGet("getbyid/{id}")]
        public IActionResult GetById(int id)
        {
            var result = _productService.GetById(id);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("getbycategoryid/{categoryId}")]
        public IActionResult GetByCategoryId(int categoryId)
        {
            var result = _productService.GetByCategoryId(categoryId);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("getbyprice")]
        public IActionResult GetByPrice(int minPrice, int maxPrice)
        {
            var result = _productService.GetByPrice(minPrice, maxPrice);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("add")]
        public IActionResult Add(Product product)
        {
            var result = _productService.Add(product);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPut("update")]
        public IActionResult Update(Product product)
        {
            var result = _productService.Update(product);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpDelete("delete")]
        public IActionResult Delete(int id)
        {
            var result = _productService.Delete(id);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("page")]
        public IActionResult GetPage(int page)
        {
            var firstIndex = (page-1) * 20;
            var result = _productService.GetProductsWithDetails();
            List<ProductDetailDto> pageResult = null;

            if (firstIndex + 20 > result.Data.Count)
            {
                var lastPageCount = result.Data.Count % 20;
                pageResult = result.Data.GetRange(firstIndex, lastPageCount);
            }
            else
            {
                pageResult = result.Data.GetRange(firstIndex, 20);
            }
            
            if (result.IsSuccess)
            {
                return Ok(pageResult);
            }

            return BadRequest(result);
        }

        [HttpPost("filter")]
        public IActionResult FilterWithName(string name)
        {
            var result = _productService.FilterWithName(name);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("filter/{page}")]
        public IActionResult FilterPage(int page, string name)
        {
            var firstIndex = (page - 1) * 20;
            var result = _productService.FilterWithName(name);
            List<ProductDetailDto> pageResult = null;


            if (firstIndex + 20 > result.Data.Count)
            {
                var lastPageCount = result.Data.Count % 20;
                pageResult = result.Data.GetRange(firstIndex, lastPageCount);
            }
            else
            {
                pageResult = result.Data.GetRange(firstIndex, 20);
            }

            if (result.IsSuccess)
            {
                return Ok(pageResult);
            }

            return BadRequest(result);
        }

        [HttpGet("getproductswithdetails")]
        public IActionResult GetProductsWithDetails()
        {
            var result = _productService.GetProductsWithDetails();
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("getoneproductwithdetails/{id}")]
        public IActionResult GetOneProductWithDetails(int id)
        {
            var result = _productService.GetOneProductWithDetails(id);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("addwithdto")]
        public IActionResult AddWithDto(ProductPostDto product)
        {
            var result = _productService.AddWithDto(product);
            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPut("updatewithdto")]
        public IActionResult UpdateWithDto(ProductPostDto product)
        {
            var result = _productService.UpdateWithDto(product);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
