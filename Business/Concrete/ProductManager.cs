using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entity.Concrete;
using Entity.DTOs;

namespace Business.Concrete
{
    public class ProductManager : IProductService
    {
        private IProductDal _productDal;
        public ProductManager(IProductDal productDal)
        {
            _productDal = productDal;
        }

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

        public IResult Delete(Product product)
        {
            _productDal.Delete(product);
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

        public IDataResult<List<Product>> FilterWithName(string filter)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.ProductName.Contains(filter)));
        }

        public IDataResult<List<ProductDetailDto>> GetProductsWithDetails()
        {
            return new SuccessDataResult<List<ProductDetailDto>>(_productDal.GetProductsWithDetails());
        }

        public IDataResult<ProductDetailDto> GetOneProductWithDetails(int id)
        {
            return new SuccessDataResult<ProductDetailDto>(_productDal.GetOneProductWithDetails(id));
        }
    }
}
