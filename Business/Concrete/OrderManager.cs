using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entity.Concrete;
using Entity.DTOs;

namespace Business.Concrete
{
    public class OrderManager : IOrderService
    {
        private IOrderDal _orderDal;
        private IOrderDetailDal _orderDetailDal;
        public OrderManager(IOrderDal orderDal, IOrderDetailDal orderDetailDal)
        {
            _orderDal = orderDal;
            _orderDetailDal = orderDetailDal;
        }

        public IResult Add(Order order)
        {
            _orderDal.Add(order);
            return new SuccessResult();
        }

        public IResult Update(Order order)
        {
            _orderDal.Update(order);
            return new SuccessResult();
        }

        public IResult Delete(Order order)
        {
            _orderDal.Delete(order);
            return new SuccessResult();
        }

        public IDataResult<List<Order>> GetAll()
        {
            return new SuccessDataResult<List<Order>>(_orderDal.GetAll());
        }

        public IDataResult<List<Order>> GetByUserId(string id)
        {
            return new SuccessDataResult<List<Order>>(_orderDal.GetAll(o => o.UserId == id));
        }

        public IDataResult<Order> GetById(int id)
        {
            return new SuccessDataResult<Order>(_orderDal.Get(o => o.OrderId == id));
        }

        public IDataResult<List<OrderDetailDto>> GetOrderWithDetails(int id)
        {
            return new SuccessDataResult<List<OrderDetailDto>>(_orderDal.GetOrderWithDetails(id));
        }

        public IResult CreateOrder(OrderPostDto order)
        {
            var orderToAdd = new Order
            {
                OrderId = 0,
                UserId = order.UserId,
                CustomerFirstName = order.CustomerFirstName,
                CustomerLastName = order.CustomerLastName,
                CustomerAddress = order.CustomerAddress,
                CustomerPhone = order.CustomerPhone,
                OrderDate = DateTime.Now
            };
            _orderDal.Add(orderToAdd);
            var orderId = _orderDal.GetAll().Last().OrderId;
            foreach (var orderDetailToAdd in order.OrderDetails.Select(orderDetail => new OrderDetail
                     {
                         OrderDetailId = 0,
                         OrderId = orderId,
                         ProductId = orderDetail.ProductId,
                         Quantity = orderDetail.Quantity,
                     }))
            {
                _orderDetailDal.Add(orderDetailToAdd);
            }

            return new SuccessResult();

        }

        public IDataResult<List<OrderDto>> GetOrders()
        {
            return new SuccessDataResult<List<OrderDto>>(_orderDal.GetOrders());
        }

        public IDataResult<List<OrderDto>> GetOrdersUser(string userId)
        {
            return new SuccessDataResult<List<OrderDto>>(_orderDal.GetOrders(o => o.UserId == userId));
        }
    }
}
