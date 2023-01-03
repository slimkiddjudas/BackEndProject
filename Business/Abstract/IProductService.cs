using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;
using Core.Utilities.Results;
using Entity.Concrete;
using Entity.DTOs;

namespace Business.Abstract
{
    public interface IProductService
    {
        IResult Add(Product product);
        IResult Update(Product product);
        IResult Delete(Product product);
        IDataResult<List<Product>> GetAll();
        IDataResult<Product> GetById(int id);
        IDataResult<List<Product>> GetByCategoryId(int id);
        IDataResult<List<Product>> GetByPrice(double minPrice, double maxPrice);
        IDataResult<List<Product>> FilterWithName(string filter);
        IDataResult<List<ProductDetailDto>> GetProductsWithDetails();
        IDataResult<ProductDetailDto> GetOneProductWithDetails(int id);
        IResult AddWithDto(ProductDetailDto product);
    }
}
