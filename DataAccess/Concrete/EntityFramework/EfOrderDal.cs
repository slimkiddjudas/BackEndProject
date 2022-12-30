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
    public class EfOrderDal : EfEntityRepositoryBase<Order, ContextDb>, IOrderDal
    {
        public List<OrderDetailDto> GetOrderWithDetails(int id)
        {
            using (ContextDb context = new ContextDb())
            {
                var result = from order in context.Orders
                    join orderDetail in context.OrderDetails on order.OrderId equals orderDetail.OrderId
                    join product in context.Products on orderDetail.ProductId equals product.ProductId
                    where order.OrderId == id
                    select new OrderDetailDto
                    {
                        ProductName = product.ProductName,
                        Quantity = orderDetail.Quantity,
                        UnitPrice = product.UnitPrice
                    };
                return result.ToList();
            }
        }
    }
}
