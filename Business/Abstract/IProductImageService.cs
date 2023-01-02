using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entity.Concrete;

namespace Business.Abstract
{
    public interface IProductImageService
    {
        IResult Add(ProductImage productImage);
        IResult Update(ProductImage productImage);
        IResult Delete(ProductImage productImage);
        IDataResult<List<ProductImage>> GetAll();
        IDataResult<ProductImage> GetById(int id);
        IDataResult<List<ProductImage>> GetByProductId(int id);
    }
}
