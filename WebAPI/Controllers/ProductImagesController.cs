using Business.Abstract;
using Core.Utilities.Results;
using Entity.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductImagesController : ControllerBase
    {
        //private readonly IProductImageService _productImageService;
        //public ProductImagesController(IProductImageService productImageService)
        //{
        //    _productImageService = productImageService;
        //}

        [HttpPost]
        public ActionResult UploadFile([FromForm] ProductImage productImage)
        {
            try
            {
                string path = Path.Combine(@"C:\Users\umut\Desktop\ProductImages", productImage.FileName);
                using (Stream stream = new FileStream(path, FileMode.Create))
                {
                    productImage.File.CopyTo(stream);
                }

                return Ok(new SuccessResult());
            }
            catch (Exception e)
            {
                return BadRequest(new ErrorResult());
            }
        }
    }
}
