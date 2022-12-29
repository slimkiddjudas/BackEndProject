using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entity.Concrete;
using Entity.DTOs;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfProductDal : EfEntityRepositoryBase<Product, ContextDb>, IProductDal
    {
        public List<ProductDetailDto> GetProductsWithDetails()
        {
            using (ContextDb context = new ContextDb())
            {
                var imagesList = from product in context.Products
                    join productImage in context.ProductImages on product.ProductId equals productImage.ProductId
                    select productImage.File;
                var result = from product in context.Products
                    join category in context.Categories on product.CategoryId equals category.CategoryId
                    select new ProductDetailDto
                    {
                        ProductName = product.ProductName,
                        CategoryName = category.CategoryName,
                        UnitPrice = product.UnitPrice,
                        UnitsInStock = product.UnitsInStock,
                        Images = imagesList.ToList()
                    };
                return result.ToList();
            }
        }
    }
}
