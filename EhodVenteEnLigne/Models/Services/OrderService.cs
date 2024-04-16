using EhodBoutiqueEnLigne.Models.Entities;
using EhodBoutiqueEnLigne.Models.Repositories;
using EhodBoutiqueEnLigne.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Order = EhodBoutiqueEnLigne.Models.Entities.Order;

namespace EhodBoutiqueEnLigne.Models.Services
{
    public class OrderService : IOrderService
    {
        private readonly ICart _cart;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductService _productService;

        public OrderService(ICart cart, IOrderRepository orderRepository, IProductService productService)
        {
            _orderRepository = orderRepository;
            _cart = cart;
            _productService = productService;
        }
        public async Task<Order> GetOrder(int id)
        {
            var orderEntity = await _orderRepository.GetOrder(id);
            return orderEntity;
        }
        public async Task<IList<Order>> GetOrders()
        {
            var orders = await _orderRepository.GetOrders();
            return orders;
        }
        public void SaveOrder(OrderViewModel order)
        {
            var orderToAdd = MapToOrderEntity(order);
            _orderRepository.Save(orderToAdd);
             UpdateInventory();
        }

        private static Order MapToOrderEntity(OrderViewModel order)
        {
            Order orderToAdd = new Order
            {
                Name = order.Name,
                Address = order.Address,
                City = order.City,
                Zip = order.Zip,
                Country = order.Country,
                Date = DateTime.UtcNow,
                OrderLine = new List<OrderLine>()
            };
            foreach (var orderLine in order.Lines)
            {
                OrderLine lineOrder = new OrderLine { ProductId = orderLine.Product.Id, Quantity = orderLine.Quantity };
                orderToAdd.OrderLine.Add(lineOrder);
            }

            return orderToAdd;
        }

        public List<string> VerifierETatDeLaCommande(OrderViewModel order)
        {
            List<string> modelErrors = new List<string>();

            if (string.IsNullOrWhiteSpace(order.Name))
            {
                modelErrors.Add("ErrorMissingName");
            }

            if (string.IsNullOrWhiteSpace(order.Address))
            {
                modelErrors.Add("ErrorMissingAddress");
            }

            if (string.IsNullOrWhiteSpace(order.City))
            {
                modelErrors.Add("ErrorMissingCity");
            }

            if (string.IsNullOrWhiteSpace(order.Zip))
            {
                modelErrors.Add("ErrorMissingZipCode");
            }

            if (string.IsNullOrWhiteSpace(order.Country))
            {
                modelErrors.Add("ErrorMissingCountry");
            }

            return modelErrors;
        }


        private void UpdateInventory()
        {
            _productService.UpdateProductQuantities();
            _cart.Clear();
        }
    }
}
