using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.Results;
using Entity.Concrete;
using Entity.DTOs;

namespace Business.Abstract
{
    public interface IOrderService
    {
        IResult Add(Order order);
        IResult Update(Order order);
        IResult Delete(Order order);
        IDataResult<List<Order>> GetAll();
        IDataResult<List<Order>> GetByUserId(string id);
        IDataResult<Order> GetById(int id);
        IDataResult<List<OrderDetailDto>> GetOrderWithDetails(int id);
        IResult CreateOrder(OrderPostDto order);
        IDataResult<List<OrderDto>> GetOrders();
        IDataResult<List<OrderDto>> GetOrdersUser(string userId);
    }
}
