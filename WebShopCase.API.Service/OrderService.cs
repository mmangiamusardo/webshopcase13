using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebShopCase.API.Data.Infrastructure;
using WebShopCase.API.Data.Repositories;
using WebShopCase.API.Domain;

namespace WebShopCase.API.Service
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository orderRepository;
        private readonly IUnitOfWork unitOfWork;

        public OrderService(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
        {
            this.orderRepository = orderRepository;
            this.unitOfWork = unitOfWork;
        }

        public Order GetOrder(int id)
        {
            return orderRepository.GetSingle(o => o.OrderID == id, o => o.OrderDetails);
        }

        public void CreateOrder(Order order)
        {
            orderRepository.Add(order);
            unitOfWork.Commit();
        }

        public IList<Order> GetOrdersByCustomer(int customerID)
        {
            return orderRepository.GetMany(o => o.CustomerID == customerID, o => o.Customer);
        }
    }

    public interface IOrderService 
    {
        Order GetOrder(int id);
        void CreateOrder(Order order);
    }
}
