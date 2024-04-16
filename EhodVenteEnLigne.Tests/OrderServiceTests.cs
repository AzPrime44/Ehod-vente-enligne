using EhodBoutiqueEnLigne.Models;
using EhodBoutiqueEnLigne.Models.Repositories;
using EhodBoutiqueEnLigne.Models.Services;
using EhodBoutiqueEnLigne.Models.ViewModels;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace EhodBoutiqueEnLigne.Tests
{
    public class OrderServiceTests
    {
        private readonly Mock<ICart> _mockForCart;
        private readonly Mock<IOrderRepository> _mockForOrderRepository;
        private readonly Mock<IProductService> _mockForProductService;
        private readonly OrderService _orderService;

        public OrderServiceTests()
        {
            _mockForCart = new Mock<ICart>();
            _mockForOrderRepository = new Mock<IOrderRepository>();
            _mockForProductService = new Mock<IProductService>();

            _orderService = new OrderService(_mockForCart.Object, _mockForOrderRepository.Object, _mockForProductService.Object);
        }

        [Fact]
        public void SaveOrder_MissingName_ShouldReturnErrorMessage()
        {
            // Arrange
            var orderViewModel = new OrderViewModel
            {
                Address = "medina",
                City = "Dakar",
                Zip = "10001",
                Country = "Senegal",
                Lines = new List<CartLine>()
            };

            // Act
            _orderService.SaveOrder(orderViewModel);
            var modelErrors = _orderService.VerifierETatDeLaCommande(orderViewModel);

            // Assert
            Assert.Contains("ErrorMissingName", modelErrors);
        }

        [Fact]
        public void SaveOrder_MissingAddress_ShouldReturnErrorMessage()
        {
            // Arrange
            var orderViewModel = new OrderViewModel
            {
                Name = "Seynabou",
                City = "Dakar",
                Zip = "10001",
                Country = "Senegal",
                Lines = new List<CartLine>(), 
            };

            // Act
            _orderService.SaveOrder(orderViewModel);
            var modelErrors = _orderService.VerifierETatDeLaCommande(orderViewModel);

            // Assert
            Assert.Contains("ErrorMissingAddress", modelErrors);
        }

        [Fact]
        public void SaveOrder_MissingCity_ShouldReturnErrorMessage()
        {
            // Arrange
            var orderViewModel = new OrderViewModel
            {
                Name = "Seynabou",
                Address = "Parcelles Assainies",
                Zip = "10001",
                Country = "Senegal",
                Lines = new List<CartLine>(),
            };

            // Act
            _orderService.SaveOrder(orderViewModel);
            var modelErrors = _orderService.VerifierETatDeLaCommande(orderViewModel);

            // Assert
            Assert.Contains("ErrorMissingCity", modelErrors);
        }

        [Fact]
        public void SaveOrder_MissingZip_ShouldReturnErrorMessage()
        {
            // Arrange
            var orderViewModel = new OrderViewModel
            {
                Name = "Seynabou",
                Address = "Parcelles Assainies",
                City = "Dakar",
                Country = "Senegal",
                Lines = new List<CartLine>(),
            };

            // Act
            _orderService.SaveOrder(orderViewModel);
            var modelErrors = _orderService.VerifierETatDeLaCommande(orderViewModel);

            // Assert
            Assert.Contains("ErrorMissingZipCode", modelErrors);
        }

        [Fact]
        public void SaveOrder_MissingCountry_ShouldReturnErrorMessage()
        {
            // Arrange
            var orderViewModel = new OrderViewModel
            {
                Name = "Seynabou",
                Address = "Parcelles Assainies",
                Zip = "10001",
                City = "Dakar",
                Lines = new List<CartLine>(),
            };

            // Act
            _orderService.SaveOrder(orderViewModel);
            var modelErrors = _orderService.VerifierETatDeLaCommande(orderViewModel);

            // Assert
            Assert.Contains("ErrorMissingCountry", modelErrors);
        }

        [Fact]
        public void CheckOrderModelErrors_AllFieldsFilled_ShouldReturnEmptyList()
        {
            // Arrange
            var orderViewModel = new OrderViewModel
            {
                Name = "Seynabou",
                Address = "Parcelles Assainies",
                City = "Dakar",
                Zip = "10001",
                Country = "Senegal",
                Lines = new List<CartLine>(), 
            };

            // Act
            var modelErrors = _orderService.VerifierETatDeLaCommande(orderViewModel);

            // Assert
            Assert.Empty(modelErrors);
        }
    }
}