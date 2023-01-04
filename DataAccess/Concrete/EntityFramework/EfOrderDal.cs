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
                //var orderDetails = context.OrderDetails.Where(od => od.OrderId == id).ToList();


                var orderWithDetails = from orderDetail in context.OrderDetails
                    join order in context.Orders on orderDetail.OrderId equals order.OrderId
                    join product in context.Products on orderDetail.ProductId equals product.ProductId
                    where orderDetail.OrderId == id
                    select new OrderDetailDto
                    {
                        ProductName = product.ProductName,
                        ProductDescription = product.Description,
                        Quantity = orderDetail.Quantity,
                        UnitPrice = product.UnitPrice,
                        OrderDate = order.OrderDate
                    };
                return orderWithDetails.ToList();
                //var result = from order in context.Orders
                //    join orderDetail in context.OrderDetails on order.OrderId equals orderDetail.OrderId
                //    join product in context.Products on orderDetail.ProductId equals product.ProductId
                //    where order.OrderId == id
                //    select new OrderDetailDto
                //    {
                //        ProductName = product.ProductName,
                //        ProductDescription = product.Description,
                //        Quantity = orderDetail.Quantity,
                //        UnitPrice = product.UnitPrice,
                //        File = null,
                //        OrderDate = order.OrderDate,
                //    };
            }
        }

        public List<OrderDto> GetOrders()
        {
            using (ContextDb context = new ContextDb())
            {
                var orders = from order in context.Orders
                    //join orderDetail in context.OrderDetails on order.OrderId equals orderDetail.OrderId
                    //join product in context.Products on orderDetail.ProductId equals product.ProductId
                    join user in context.Users on order.UserId equals user.Id
                    select new OrderDto
                    {
                        UserMail = user.Email,
                        CustomerFirstName = order.CustomerFirstName,
                        CustomerLastName = order.CustomerLastName,
                        CustomerAddress = order.CustomerAddress,
                        CustomerPhone = order.CustomerPhone,
                        OrderDate = order.OrderDate
                    };
                return orders.ToList();
            }
        }
    }
}
