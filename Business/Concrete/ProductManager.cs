using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Business.Abstract;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entity.Concrete;
using Entity.DTOs;

namespace Business.Concrete
{
    public class ProductManager : IProductService
    {
        private IProductDal _productDal;
        private IProductImageDal _productImageDal;
        private IProductImageService _productImageService;
        public ProductManager(IProductDal productDal, IProductImageDal productImageDal, IProductImageService productImageService)
        {
            _productDal = productDal;
            _productImageDal = productImageDal;
            _productImageService = productImageService;
        }

        [ValidationAspect(typeof(ProductValidator))]
        public IResult Add(Product product)
        {
            _productDal.Add(product);
            return new SuccessResult();
        }

        public IResult Update(Product product)
        {
            _productDal.Update(product);
            return new SuccessResult();
        }

        public IResult Delete(int id)
        {
            _productDal.Delete(_productDal.Get(p => p.ProductId == id));
            return new SuccessResult();
        }

        public IDataResult<List<Product>> GetAll()
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll());
        }

        public IDataResult<Product> GetById(int id)
        {
            return new SuccessDataResult<Product>(_productDal.Get(p => p.ProductId == id));
        }

        public IDataResult<List<Product>> GetByCategoryId(int id)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.CategoryId == id));
        }

        public IDataResult<List<Product>> GetByPrice(double minPrice, double maxPrice)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.UnitPrice > minPrice && p.UnitPrice < maxPrice));
        }

        public IDataResult<List<ProductDetailDto>> FilterWithName(string filter)
        {
            return new SuccessDataResult<List<ProductDetailDto>>(_productDal.GetProductsWithDetails(p => p.ProductName.Contains(filter)));
        }

        public IDataResult<List<ProductDetailDto>> GetProductsWithDetails()
        {
            return new SuccessDataResult<List<ProductDetailDto>>(_productDal.GetProductsWithDetails());
        }

        public IDataResult<ProductDetailDto> GetOneProductWithDetails(int id)
        {
            return new SuccessDataResult<ProductDetailDto>(_productDal.GetOneProductWithDetails(id));
        }

        public IResult AddWithDto(ProductDetailDto product)
        {
            var productToAdd = new Product
            {
                ProductId = 0,
                CategoryId = 1,
                ProductName = product.ProductName,
                UnitPrice = product.UnitPrice,
                UnitsInStock = product.UnitsInStock,
                Description = product.Description
            };
            _productDal.Add(productToAdd);
            var productId = GetAll().Data.Last().ProductId;
            foreach (var productImageToAdd in product.ImageUrls.Select(productImageUrl => new ProductImage
                     {
                         ProductImageId = 0,
                         ProductId = productId,
                         ImageUrl = productImageUrl.ImageUrl
                     }))
            {
                _productImageDal.Add(productImageToAdd);
            }

            return new SuccessResult();
        }

        public IResult UpdateWithDto(ProductDetailDto product)
        {
            var productToUpdate = new Product
            {
                ProductId = product.ProductId,
                CategoryId = 1,
                ProductName = product.ProductName,
                UnitPrice = product.UnitPrice,
                UnitsInStock = product.UnitsInStock,
                Description = product.Description
            };
            _productDal.Update(productToUpdate);
            foreach (var productImageUrl in product.ImageUrls)
            {
                var imagesToDelete = _productImageService.GetByProductId(product.ProductId);
                foreach (var img in imagesToDelete.Data)
                {
                    _productImageService.Delete(img);
                }
                var productImageToAdd = new ProductImage { ProductImageId = 0, ProductId = product.ProductId, ImageUrl = productImageUrl.ImageUrl };
                _productImageDal.Add(productImageToAdd);
            }

            return new SuccessResult();
        }
    }
}
