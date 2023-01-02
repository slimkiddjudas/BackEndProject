using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entity.Concrete;

namespace Business.Concrete
{
    public class ProductImageManager : IProductImageService
    {
        private readonly IProductImageDal _productImageDal;
        public ProductImageManager(IProductImageDal productImageDal)
        {
            _productImageDal = productImageDal;
        }

        public IResult Add(ProductImage productImage)
        {
            _productImageDal.Add(productImage);
            return new SuccessResult();
        }

        public IResult Update(ProductImage productImage)
        {
            _productImageDal.Update(productImage);
            return new SuccessResult();
        }

        public IResult Delete(ProductImage productImage)
        {
            _productImageDal.Delete(productImage);
            return new SuccessResult();
        }

        public IDataResult<List<ProductImage>> GetAll()
        {
            return new SuccessDataResult<List<ProductImage>>(_productImageDal.GetAll());
        }

        public IDataResult<ProductImage> GetById(int id)
        {
            return new SuccessDataResult<ProductImage>(_productImageDal.Get(p => p.ProductImageId == id));
        }

        public IDataResult<List<ProductImage>> GetByProductId(int id)
        {
            return new SuccessDataResult<List<ProductImage>>(_productImageDal.GetAll(p => p.ProductId == id));
        }
    }
}
