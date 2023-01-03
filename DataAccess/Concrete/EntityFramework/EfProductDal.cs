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
                    select productImage;
                var result = from product in context.Products
                    join category in context.Categories on product.CategoryId equals category.CategoryId
                    select new ProductDetailDto
                    {
                        ProductId = product.ProductId,
                        ProductName = product.ProductName,
                        CategoryName = category.CategoryName,
                        UnitPrice = product.UnitPrice,
                        UnitsInStock = product.UnitsInStock,
                        ImageUrls= imagesList.Where(pi => pi.ProductId == product.ProductId).ToList(),
                        Description = product.Description
                    };
                return result.ToList();
            }
        }

        public ProductDetailDto GetOneProductWithDetails(int id)
        {
            using (ContextDb context = new ContextDb())
            {
                var imagesList = from productImage in context.ProductImages
                    where productImage.ProductId == id
                    select productImage;
                var result = from product in context.Products
                    join category in context.Categories on product.CategoryId equals category.CategoryId
                    where product.ProductId == id
                    select new ProductDetailDto
                    {
                        ProductId = product.ProductId,
                        ProductName = product.ProductName,
                        CategoryName = category.CategoryName,
                        UnitPrice = product.UnitPrice,
                        UnitsInStock = product.UnitsInStock,
                        ImageUrls = imagesList.ToList(),
                        Description = product.Description
                    };
                return result.ToList()[0];
            }
        }
    }
}
