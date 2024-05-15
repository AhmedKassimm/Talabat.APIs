using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregation;
using Talabat.Core.Repositories;
using Talabat.Core.Services;
using Talabat.Core.Specifications.Order_Spec;

namespace Talabat.Service
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository basketRepository;
        private readonly IUnitOfWork unitOfWork;

        //private readonly IGenericRepository<Product> productRepo;
        //private readonly IGenericRepository<DeliveryMethod> dMRepository;
        //private readonly IGenericRepository<Order> orderRepository;

        public OrderService(IBasketRepository basketRepository,IUnitOfWork unitOfWork)
        {
            this.basketRepository = basketRepository;
            this.unitOfWork = unitOfWork;
            //productRepo = ProductRepo;
            //dMRepository = DMRepository;
            //orderRepository = OrderRepository;
        }
        

        public async Task<Order?> CreateOrderAsync(string BuyerEmail, string basketId, int deliveryMethodId, Address ShippingAddress)
        {
            //1.Get Basket From Basket Repo
            var basket = await basketRepository.GetBasketAsync(basketId);
            //2.Get Selected Item at Basket From ProductRepo
            var orderItems = new List<OrderItem>();
            if (basket?.Items?.Count > 0)
            {
                foreach (var item in basket.Items)
                {
                    var product = await unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                    var productItemOrdered = new ProductOrderItem(product.Id, product.Name, product.PictureUrl);
                    var ordeItem = new OrderItem(productItemOrdered, product.Price, item.Quantity);
                    orderItems.Add(ordeItem);
                }
            }
            // 3. Calculate Subtotal
            var subtotal = orderItems.Sum(item => item.Price * item.Quantity);
            //4.Get Delivery Method From DeliveryMetod Repo
            var deliverymethod = await unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);
            // 5.Create Order
            var Order = new Order(BuyerEmail, ShippingAddress, deliverymethod, orderItems, subtotal);
            // 6.Add Order Locally
            await unitOfWork.Repository<Order>().Add(Order); // local
            //7. Save Order to Database(Orders)
            var result= await unitOfWork.Complete();
            if (result <= 0) return null;
            return Order;
        }

        public async Task<Order> CreateOrderByIdForUserAsync(int orderId, string buyerEmail)
        {
            var spec = new OrderSpecification(orderId,buyerEmail);
            var order = await unitOfWork.Repository<Order>().GetByIdWithSpecAsync(spec);
            return order;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            var deliverymethods= await unitOfWork.Repository<DeliveryMethod>().GetAllAsync();
            return deliverymethods;
        }

        public async Task<IReadOnlyList<Order>> GetOrderForUserAsync(string buyerEmail)
        {
            
                // Create a new OrderSpecification with the provided buyerEmail
                var spec = new OrderSpecification(buyerEmail);

            // Use the specification to get orders for the user
            var orders = await unitOfWork.Repository<Order>().GetAllWithSpaceAsync(spec);

                return orders;
            
            
        }
    }
}
