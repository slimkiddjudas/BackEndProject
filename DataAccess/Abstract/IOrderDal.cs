using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Core.DataAccess;
using Entity.Concrete;
using Entity.DTOs;

namespace DataAccess.Abstract
{
    public interface IOrderDal : IEntityRepository<Order>
    {
        List<OrderDetailDto> GetOrderWithDetails(int id);
        List<OrderDto> GetOrders(Expression<Func<OrderDto, bool>> filter = null);
    }
}
